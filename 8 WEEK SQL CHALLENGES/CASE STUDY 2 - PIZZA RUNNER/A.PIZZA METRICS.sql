-- A. Pizza Metrics
-- 1.How many pizzas were ordered?
select count (order_id) as count_order
from ##new_customer_orders

-- 2.How many unique customer orders were made?
select count (distinct order_id) as count_customer
from ##new_customer_orders

-- 3.How many successful orders were delivered by each runner?
select runner_id,
       COUNT(runner_id) as count_successful
from ##new_runner_orders
where cancellation = ''
group by runner_id

-- 4.How many of each type of pizza was delivered?
select cast (pizza_name as varchar (50)) as pizza_name,
       count (c.pizza_id) as count_pizza_was_delivered
from ##new_customer_orders c
join pizza_names p on p.pizza_id = c.pizza_id
join ##new_runner_orders r on r.order_id = c.order_id
where r.cancellation = ''
group by cast (pizza_name as varchar (50))

-- 5.How many Vegetarian and Meatlovers were ordered by each customer?
select c.customer_id, cast (pizza_name as varchar (50)) as pizza_name,
       count (c.pizza_id) as count_pizza 
from ##new_customer_orders c
join pizza_names p on p.pizza_id = c.pizza_id
join ##new_runner_orders r on r.order_id = c.order_id
where r.cancellation = ''
group by cast (pizza_name as varchar (50)), c.customer_id
order by c.customer_id

-- 6.What was the maximum number of pizzas delivered in a single order?
with cte as (select c.order_id,
                    count (c.pizza_id) as count_pizza
             from ##new_customer_orders c
             join ##new_runner_orders r on r.order_id = c.order_id
             where r.cancellation = ''
             group by c.order_id)

select order_id, max (count_pizza) as max_pizza
from cte 
group by order_id;

-- 7.For each customer, how many delivered pizzas had at least 1 change and how many had no changes?
with cte as (select order_id, customer_id, pizza_id,
       case when extras != '' or exclusions !='' then 'change' else 'not change' end as 'status'
from ##new_customer_orders)

select customer_id, status, COUNT(pizza_id) as count_pizza
from cte
join ##new_runner_orders r on r.order_id = cte.order_id
where r.cancellation = ''
group by customer_id, status
order by customer_id, status

-- 8.How many pizzas were delivered that had both exclusions and extras?
select count (c.pizza_id) as count_pizza
from ##new_customer_orders c
join ##new_runner_orders r on r.order_id = c.order_id
where r.cancellation = '' and exclusions != '' and extras != ''

-- 9.What was the total volume of pizzas ordered for each hour of the day?
select datepart (hh,order_time) as hour,
       count (pizza_id) as count_pizza
from ##new_customer_orders c
join ##new_runner_orders r on r.order_id = c.order_id
where r.cancellation = ''
group by datepart (hh,order_time)

-- 10.What was the volume of orders for each day of the week?
select datename (dw,order_time) as [weekday],
       count (order_id) as count_pizza
from ##new_customer_orders
group by datename (dw,order_time)