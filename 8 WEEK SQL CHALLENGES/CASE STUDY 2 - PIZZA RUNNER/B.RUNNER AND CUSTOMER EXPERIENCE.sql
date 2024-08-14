-- B. Runner and Customer Experience
-- 1.How many runners signed up for each 1 week period? (i.e. week starts 2021-01-01)
select datepart(ww,registration_date) as week, count (runner_id) as count_runners
from runners
group by datepart(ww,registration_date);

-- 2.What was the average time in minutes it took for each runner to arrive at the Pizza Runner HQ to pickup the order?
with cte as (select runner_id, r.order_id, cast (datediff (minute, order_time, pickup_time) as float) as [time]
             from ##new_runner_orders r
             join ##new_customer_orders c on r.order_id = c.order_id
             where cancellation =''
             group by runner_id, cast (datediff (minute, order_time, pickup_time) as float), r.order_id)

select runner_id,
       round (avg([time]),2) as avg_time
from cte
group by runner_id;

-- 3.Is there any relationship between the number of pizzas and how long the order takes to prepare?
with cte as (select c.order_id, cast (datediff (minute, order_time, pickup_time) as float) as time,
                    count(pizza_id) as count_pizza
from ##new_customer_orders c
join ##new_runner_orders r on r.order_id = c.order_id
where cancellation = ''
group by c.order_id, cast (datediff (minute, order_time, pickup_time) as float))

select count_pizza,
       round (avg ([time]),2) as avg_time,
       round ((avg ([time])/count_pizza),2) as avg_time_per_pizza
from cte 
group by count_pizza

-- 4.What was the average distance travelled for each customer?
select customer_id, round (avg(cast (distance as float)),2) as avg_distance
from ##new_runner_orders r
join ##new_customer_orders c on c.order_id = r.order_id
where cancellation =''
group by customer_id;

-- 5.What was the difference between the longest and shortest delivery times for all orders?
select
       max (cast (duration as float)) as longest,
       min (cast (duration as float)) as shortest,
       max (cast (duration as float)) - min (cast (duration as float)) as difference
from ##new_runner_orders
where cancellation =''

-- 6.What was the average speed for each runner for each delivery and do you notice any trend for these values?
with cte as (select r.order_id, runner_id,
       round (avg (cast (distance as float)/cast (duration as float))*60,2) as avg_speed
from ##new_runner_orders r
join ##new_customer_orders c on c.order_id = r.order_id
where cancellation = ''
group by runner_id, r.order_id)

select runner_id,
       avg (avg_speed) as avg_speed_by_runner
from cte
group by runner_id;
---> AVG of runner_id is max

-- 7.What is the successful delivery percentage for each runner?
with cte as (select runner_id, 
       count (order_id) as total_count,
       case when cancellation = '' then count (order_id) else 0 end as count_successful
from ##new_runner_orders
group by runner_id, order_id, cancellation)

select runner_id,
     sum(count_successful)*100/sum(total_count) as successful_percentage
from cte
group by runner_id