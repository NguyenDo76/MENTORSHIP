--B.DATA ANALYSIS QUESTIONS
-- 1.How many customers has Foodie-Fi ever had?
select count (distinct customer_id) as count_customer
from foodie_fi.subscriptions;

-- 2.What is the monthly distribution of trial plan start_date values for our dataset - use the start of the month as the group by value
select date_part ('month', s.start_date) as month, 
       count (distinct customer_id) as count_customer
from foodie_fi.subscriptions s
join foodie_fi.plans p on p.plan_id = s.plan_id
where p.plan_id = 0
group by date_part ('month', s.start_date);

-- 3.What plan start_date values occur after the year 2020 for our dataset? Show the breakdown by count of events for each plan_name
select p.plan_id, p.plan_name, 
       count (s.customer_id) count_customer
from foodie_fi.subscriptions s
join foodie_fi.plans p on p.plan_id = s.plan_id
where date_part ('year', s.start_date) > 2020
group by p.plan_id, p.plan_name
order by p.plan_id;

-- 4.What is the customer count and percentage of customers who have churned rounded to 1 decimal place?
with cte as (select count (distinct customer_id) as total_count_cus
             from foodie_fi.subscriptions s),

     cte1 as (select count (distinct customer_id) as count_churned_cus
              from foodie_fi.subscriptions s
              join foodie_fi.plans p on p.plan_id = s.plan_id
              where p.plan_id = 4)

select count_churned_cus,
	round (count_churned_cus*100.0/total_count_cus,1) as percentage
from cte,cte1;

-- 5.How many customers have churned straight after their initial free trial - what percentage is this rounded to the nearest whole number?
with cte as (select customer_id, plan_id,
	             lead (plan_id,1) over (partition by customer_id order by plan_id) AS next_plan_id
             from foodie_fi.subscriptions)

select count (customer_id) as count_churned,
	round (count (next_plan_id)*100.0/(select count (distinct customer_id) from foodie_fi.subscriptions),0) as percentage
from cte
where plan_id = 0 and next_plan_id = 4;

-- 6.What is the number and percentage of customer plans after their initial free trial?
with cte as (select customer_id, plan_id,
		      lead (plan_id,1) over (partition by customer_id order by plan_id) AS next_plan_id
	      from foodie_fi.subscriptions)

select next_plan_id,
	count (customer_id) as count,
	round (count (next_plan_id)*100.0/(select count (distinct customer_id) from foodie_fi.subscriptions),2) as percentage
from cte
where next_plan_id is not null
group by next_plan_id
order by next_plan_id;

--7.What is the customer count and percentage breakdown of all 5 plan_name values at 2020-12-31?
with cte as (select *,
                    lead (start_date,1) over (partition by customer_id order by plan_id) AS next_start_date
             from foodie_fi.subscriptions
             where start_date <= '2020-12-31')

select cte.plan_id,p.plan_name, 
	count (distinct customer_id) as count_customer,
       round(count (distinct customer_id) * 100.0/(select count (distinct customer_id) from foodie_fi.subscriptions),2) as percentage
from cte
join foodie_fi.plans p on p.plan_id = cte.plan_id
where next_start_date is null or next_start_date > '2020-12-31' -- is null is don't know after plan and next date is next after plan
group by cte.plan_id,p.plan_name;

--8.How many customers have upgraded to an annual plan in 2020?
select count (distinct customer_id) as count_cus_annual
from foodie_fi.subscriptions
where plan_id = 3 and start_date <= '2020-12-31';

--9.How many days on average does it take for a customer to an annual plan from the day they join Foodie-Fi?
with cte_trial as (select s.*
                   from foodie_fi.subscriptions s
                   join foodie_fi.plans p on p.plan_id = s.plan_id
                   where plan_name = 'trial'),

      cte_annual as (select s.*
                     from foodie_fi.subscriptions s
                     join foodie_fi.plans p on p.plan_id = s.plan_id
                     where plan_name = 'pro annual')

select round (avg (a.start_date - t.start_date),0) as avg_day
from cte_trial t
join cte_annual a on t.customer_id = a.customer_id;

--10.Can you further breakdown this average value into 30 day periods (i.e. 0-30 days, 31-60 days etc)
with cte_trial as (select s.*
                   from foodie_fi.subscriptions s
                   join foodie_fi.plans p on p.plan_id = s.plan_id
                   where plan_name = 'trial'),

      cte_annual as (select s.*
                     from foodie_fi.subscriptions s
                     join foodie_fi.plans p on p.plan_id = s.plan_id
                     where plan_name = 'pro annual'),
                     
      cte_diff_day as (select t.customer_id,
                              a.start_date - t.start_date as diff_day
                       from cte_trial t
                       join cte_annual a on t.customer_id = a.customer_id),

       cte_group_day as (select*, floor(diff_day/30) as group_day
                         from cte_diff_day)
                
select concat ((group_day *30) + 1 , ' - ' , (group_day + 1) * 30 , ' days') as day_range,
       count (group_day) as avg_day
from cte_group_day
group by group_day
order by group_day;

--11.How many customers downgraded from a pro monthly to a basic monthly plan in 2020?
with cte as (select customer_id, plan_id,
	             lead (plan_id,1) over (partition by customer_id order by plan_id) AS next_plan_id
             from foodie_fi.subscriptions
            where start_date <= '2020-12-31')

select count (distinct customer_id) as count_customer
from cte
where plan_id = 2 and next_plan_id = 1