# :tada: CASE STUDY #3 - FOODIE-FI

Find the full case study [**here**](https://8weeksqlchallenge.com/case-study-3/).

## :books: Table of Contents
 - [Introduction](#introduction)
 - [Entity Relationship Diagram](#entity)
 - [Example Datasets](#example)
 - [Questions and Solution](#solution)
   - [A. Customer Journey](#customer)
   - [B. Data Analysis Questions](#analysis)
   - [C. Challenge Payment Question](#payment)
   - [D. Outside The Box Questions](#outside)


<a name="introduction"></a>
## :question: Introduction

Subscription based businesses are super popular and Danny realised that there was a large gap in the market - he wanted to create a new streaming service that only had food related content - something like Netflix but with only cooking shows!

Danny finds a few smart friends to launch his new startup Foodie-Fi in 2020 and started selling monthly and annual subscriptions, giving their customers unlimited on-demand access to exclusive food videos from around the world!

Danny created Foodie-Fi with a data driven mindset and wanted to ensure all future investment decisions and new features were decided using data. This case study focuses on using subscription style digital data to answer important business questions.

<a name="entity"></a>
## :bookmark: Entity Relationship Diagram

<a name="example"></a>
## :open_book: Example Datasets

**Table 1: Plans**

| plan_id | plan_name     | price |
| :------ | :------------ | :---- |
| 0       | trial         | 0     |
| 1       | basic monthly | 9.90  |
| 2       | pro monthly   | 19.90 |
| 3       | pro annual    | 199   |
| 4       | churn         | null  |

**Table 2: Subscriptions**

| customer_id | plan_id | start_date |
| :---------- | :------ | :--------- |
| 1           | 0       | 2020-08-01 |
| 1           | 1       | 2020-08-08 |
| 2           | 0       | 2020-09-20 |
| 2           | 3       | 2020-09-27 |
| 11          | 0       | 2020-11-19 |
| 11          | 4       | 2020-11-26 |
| 13          | 0       | 2020-12-15 |
| 13          | 1       | 2020-12-22 |
| 13          | 2       | 2021-03-29 |
| 15          | 0       | 2020-03-17 |
| 15          | 2       | 2020-03-24 |
| 15          | 4       | 2020-04-29 |
| 16          | 0       | 2020-05-31 |
| 16          | 1       | 2020-06-07 |
| 16          | 3       | 2020-10-21 |
| 18          | 0       | 2020-07-06 |
| 18          | 2       | 2020-07-13 |
| 19          | 0       | 2020-06-22 |
| 19          | 2       | 2020-06-29 |
| 19          | 3       | 2020-08-29 |

<a name="solution"></a>
## :boom: Questions and Solution
<a name="customer"></a>
### **A. Customer Journey**

Based off the 8 sample customers provided in the sample from the subscriptions table, write a brief description about each customer’s onboarding journey.

Try to keep it as short as possible - you may also want to run some sort of join to make your explanations a bit easier!

<a name="analysis"></a>
### **B. Data Analysis Questions**

 - **Question 1: How many customers has Foodie-Fi ever had?**

```tsql
select count (distinct customer_id) as count_customer
from [foodie_fi.subscriptions];
```
|count_customer|
|:------|
|1000|

 - **Question 2: What is the monthly distribution of trial plan start_date values for our dataset - use the start of the month as the group by value**

```tsql
select datepart (month, s.start_date) as month, 
       count (distinct customer_id) as count_customer
from [foodie_fi.subscriptions] s
join [foodie_fi.plans] p on p.plan_id = s.plan_id
where p.plan_id = 0
group by datepart (month, s.start_date);
```

|month|count_customer|
|---|---|
|1|88|
|2|68|
|3|94|
|4|81|
|5|88|
|6|79|
|7|89|
|8|88|
|9|87|
|10|79|
|11|75|
|12|84|

 - **Question 3: What plan start_date values occur after the year 2020 for our dataset? Show the breakdown by count of events for each plan_name**

```tsql
select p.plan_id, p.plan_name, 
       count (s.customer_id) count_customer
from [foodie_fi.subscriptions] s
join [foodie_fi.plans] p on p.plan_id = s.plan_id
where datepart (year, s.start_date) > 2020
group by p.plan_id, p.plan_name
order by p.plan_id;
```

|plan_id|plan_name|count_customer|
|---|---|---|
|1|basicmonthly|8|
|2|promonthly|60|
|3|proannual|63|
|4|churn|71|

 - **Question 4: What is the customer count and percentage of customers who have churned rounded to 1 decimal place?**

```tsql
with cte_count_cus as (select count (distinct customer_id) as total_count_cus
                       from [foodie_fi.subscriptions]),

     cte_count_churned as (select count (distinct customer_id) as count_churned_cus
                           from [foodie_fi.subscriptions] s
                           join [foodie_fi.plans] p on p.plan_id = s.plan_id
                           where p.plan_id = 4)

select count_churned_cus,
       cast (round ((count_churned_cus*100.0/total_count_cus),1) as decimal (10,1)) as percentage
from cte_count_cus,cte_count_churned;
```

|count_churned_cus|percentage|
|---|---|
|307|30.7|

 - **Question 5: How many customers have churned straight after their initial free trial - what percentage is this rounded to the nearest whole number?**

```tsql
with cte_next_plan as (select customer_id, plan_id,
	                            lead (plan_id,1) over (partition by customer_id order by plan_id) AS next_plan_id
                       from [foodie_fi.subscriptions])

select count (customer_id) as count_churned,
       cast (round (count (next_plan_id)*100.0/(select count (distinct customer_id) from [foodie_fi.subscriptions]),0) as decimal (10,1)) as percentage
from cte_next_plan
where plan_id = 0 and next_plan_id = 4;
```
|count_churned|percentage|
|---|---|
|92|9.0|

 - **Question 6: What is the number and percentage of customer plans after their initial free trial?**

```tsql
with cte_next_plan as (select customer_id, plan_id,
		                          lead (plan_id,1) over (partition by customer_id order by plan_id) AS next_plan_id
	                     from [foodie_fi.subscriptions])

select next_plan_id,
       count (customer_id) as count,
       cast (round (count (next_plan_id)*100.0/(select count (distinct customer_id) from [foodie_fi.subscriptions]),2) as decimal (10,2)) as percentage
from cte_next_plan
where next_plan_id is not null
group by next_plan_id
order by next_plan_id;
```

|next_plan_id|count|percentage|
|---|---|---|
|1|546|54.60|
|2|539|53.90|
|3|258|25.80|
|4|307|30.70|


 - **Question 7: What is the customer count and percentage breakdown of all 5 plan_name values at 2020-12-31?**

```tsql

with cte_next_start_date as (select *,
                                    lead (start_date,1) over (partition by customer_id order by plan_id) AS next_start_date
                             from [foodie_fi.subscriptions]
                             where start_date <= '2020-12-31')

select cte.plan_id, p.plan_name, 
	     count (distinct customer_id) as count_customer,
       cast (round(count (distinct customer_id) * 100.0/(select count (distinct customer_id) from [foodie_fi.subscriptions]),2) as decimal (10,2)) as percentage
from cte_next_start_date cte
join [foodie_fi.plans] p on p.plan_id = cte.plan_id
where next_start_date is null or next_start_date > '2020-12-31' -- is null is don't know after plan and next date is next after plan
group by cte.plan_id,p.plan_name;
```

|plan_id|plan_name|count_customer|percentage|
|---|---|---|---|
|1|basicmonthly|224|22.40|
|4|churn|236|23.60|
|3|proannual|195|19.50|
|2|promonthly|326|32.60|
|0|trial|19|1.90|


 - **Question 8: How many customers have upgraded to an annual plan in 2020?**

```tsql

select count (distinct customer_id) as count_cus_annual
from [foodie_fi.subscriptions]
where plan_id = 3 and start_date <= '2020-12-31';
```

|count_cus_annual|
|---|
|195|

 - **Question 9: How many days on average does it take for a customer to an annual plan from the day they join Foodie-Fi?**

```tsql
with cte_trial as (select s.*
                   from [foodie_fi.subscriptions] s
                   join [foodie_fi.plans] p on p.plan_id = s.plan_id
                   where plan_name = 'trial'),

     cte_annual as (select s.*
                    from [foodie_fi.subscriptions] s
                    join [foodie_fi.plans] p on p.plan_id = s.plan_id
                    where plan_name = 'pro annual')

select round (avg (datediff (day, t.start_date,a.start_date)),0) as avg_day
from cte_trial t
join cte_annual a on t.customer_id = a.customer_id;
```
|avg_day|
|---|
|104|

 - **Question 10: Can you further breakdown this average value into 30 day periods (i.e. 0-30 days, 31-60 days etc)**

```tsql
with cte_trial as (select s.*
                   from [foodie_fi.subscriptions] s
                   join [foodie_fi.plans] p on p.plan_id = s.plan_id
                   where plan_name = 'trial'),

     cte_annual as (select s.*
                    from [foodie_fi.subscriptions] s
                    join [foodie_fi.plans] p on p.plan_id = s.plan_id
                    where plan_name = 'pro annual'),
                     
     cte_diff_day as (select t.customer_id,
                             datediff (day,t.start_date, a.start_date)  as diff_day
                      from cte_trial t
                      join cte_annual a on t.customer_id = a.customer_id),

     cte_group_day as (select*,
                             floor(diff_day/30) as group_day
                       from cte_diff_day)
                
select concat ((group_day *30) + 1 , ' - ' , (group_day + 1) * 30 , ' days') as day_range,
       count (group_day) as avg_day
from cte_group_day
group by group_day
order by group_day;
```

|day_range|avg_day|
|---|---|
|1 - 30 days|48|
|31 - 60 days|25|
|61 - 90 days|33|
|91 - 120 days|35|
|121 - 150 days|43|
|151 - 180 days|35|
|181 - 210 days|27|
|211 - 240 days|4|
|241 - 270 days|5|
|271 - 300 days|1|
|301 - 330 days|1|
|331 - 360 days|1|

 - **Question 11: How many customers downgraded from a pro monthly to a basic monthly plan in 2020?**

```tsql
with cte_next_plan as (select customer_id, plan_id,
	                            lead (plan_id,1) over (partition by customer_id order by plan_id) AS next_plan_id
                       from [foodie_fi.subscriptions]
                       where start_date <= '2020-12-31')

select count (distinct customer_id) as count_customer
from cte_next_plan
where plan_id = 2 and next_plan_id = 1
```

|count_customer|
|---|
|0|

<a name="payment"></a>
### **C. Challenge Payment Question**

  **The Foodie-Fi team wants you to create a new payments table for the year 2020 that includes amounts paid by each customer in the subscriptions table with the following requirements:**
   - **monthly payments always occur on the same day of month as the original start_date of any monthly paid plan**
   - **upgrades from basic to monthly or pro plans are reduced by the current paid amount in that month and start immediately**
   - **upgrades from pro monthly to pro annual are paid at the end of the current billing period and also starts at the end of the month period**
   - **once a customer churns they will no longer make payments**
  
   **Example outputs for this table might look like the following:**

| customer_id | plan_id | plan_name     | payment_date | amount | payment_order |
|-------------|---------|---------------|--------------|--------|---------------|
| 1           | 1       | basic monthly | 2020-08-08   | 9.90   | 1             |
| 1           | 1       | basic monthly | 2020-09-08   | 9.90   | 2             |
| 1           | 1       | basic monthly | 2020-10-08   | 9.90   | 3             |
| 1           | 1       | basic monthly | 2020-11-08   | 9.90   | 4             |
| 1           | 1       | basic monthly | 2020-12-08   | 9.90   | 5             |
| 2           | 3       | pro annual    | 2020-09-27   | 199.00 | 1             |
| 13          | 1       | basic monthly | 2020-12-22   | 9.90   | 1             |
| 15          | 2       | pro monthly   | 2020-03-24   | 19.90  | 1             |
| 15          | 2       | pro monthly   | 2020-04-24   | 19.90  | 2             |
| 16          | 1       | basic monthly | 2020-06-07   | 9.90   | 1             |
| 16          | 1       | basic monthly | 2020-07-07   | 9.90   | 2             |
| 16          | 1       | basic monthly | 2020-08-07   | 9.90   | 3             |
| 16          | 1       | basic monthly | 2020-09-07   | 9.90   | 4             |
| 16          | 1       | basic monthly | 2020-10-07   | 9.90   | 5             |
| 16          | 3       | pro annual    | 2020-10-21   | 189.10 | 6             |
| 18          | 2       | pro monthly   | 2020-07-13   | 19.90  | 1             |
| 18          | 2       | pro monthly   | 2020-08-13   | 19.90  | 2             |
| 18          | 2       | pro monthly   | 2020-09-13   | 19.90  | 3             |
| 18          | 2       | pro monthly   | 2020-10-13   | 19.90  | 4             |
| 18          | 2       | pro monthly   | 2020-11-13   | 19.90  | 5             |
| 18          | 2       | pro monthly   | 2020-12-13   | 19.90  | 6             |
| 19          | 2       | pro monthly   | 2020-06-29   | 19.90  | 1             |
| 19          | 2       | pro monthly   | 2020-07-29   | 19.90  | 2             |
| 19          | 3       | pro annual    | 2020-08-29   | 199.00 | 3             |



<a name="outside"></a>
### **D. Outside The Box Questions**

**The following are open ended questions which might be asked during a technical interview for this case study - there are no right or wrong answers, but answers that make sense from both a technical and a business perspective make an amazing impression!**
 - **Question 1: How would you calculate the rate of growth for Foodie-Fi?**
 - **Question 2: What key metrics would you recommend Foodie-Fi management to track over time to assess performance of their overall business?**
 - **Question 3: What are some key customer journeys or experiences that you would analyse further to improve customer retention?**
 - **Question 4: If the Foodie-Fi team were to create an exit survey shown to customers who wish to cancel their subscription, what questions would you include in the survey?**
 - **Question 5: What business levers could the Foodie-Fi team use to reduce the customer churn rate? How would you validate the effectiveness of your ideas?**


