# :tada: CASE STUDY #2 - PIZZA RUNNER

Find the full case study [**here**](https://8weeksqlchallenge.com/case-study-2/).

## :books: Table of Contents
 - [Introduction](#introduction)
 - [Entity Relationship Diagram](#entity)
 - [Example Datasets](#example)
 - [Questions and Solution](#solution)
   - [Data Cleaning](#datacleaning)
   - [A. Pizza Metrics](#metrics)
   - [B. Runner and Customer Experience](#experience)
   - [C. Ingredient Optimisation](#ingredient)
   - [D. Pricing and Ratings](#pricing)
   - [E. Bonus Questions](bonus)


---
<a name="introduction"></a>
## :question: Introduction

Did you know that over 115 million kilograms of pizza is consumed daily worldwide??? (Well according to Wikipedia anyway…)

Danny was scrolling through his Instagram feed when something really caught his eye - “80s Retro Styling and Pizza Is The Future!”

Danny was sold on the idea, but he knew that pizza alone was not going to help him get seed funding to expand his new Pizza Empire - so he had one more genius idea to combine with it - he was going to Uberize it - and so Pizza Runner was launched!

Danny started by recruiting “runners” to deliver fresh pizza from Pizza Runner Headquarters (otherwise known as Danny’s house) and also maxed out his credit card to pay freelance developers to build a mobile app to accept orders from customers.


---
<a name="entity"></a>
## :bookmark: Entity Relationship Diagram

<img src="https://github.com/NguyenDo76/MENTORSHIP/blob/main/8%20WEEK%20SQL%20CHALLENGES/IMAGE/CASE%20STUDY%202%20-%20PIZZA%20RUNNER.png">


---
<a name="example"></a>
## :open_book: Example Datasets

**Table 1: runner**
| runner_id | registration_date |
| :-------- | :---------------- |
| 1         | 2021-01-01        |
| 2         | 2021-01-03        |
| 3         | 2021-01-08        |
| 4         | 2021-01-15        |



**Table 2: customer_orders**

| order_id | customer_id | pizza_id | exclusions | extras | order_time          |
| :------- | :---------- | :------- | :--------- | :----- | :------------------ |
| 1        | 101         | 1        |            |        | 2021-01-01 18:05:02 |
| 2        | 101         | 1        |            |        | 2021-01-01 19:00:52 |
| 3        | 102         | 1        |            |        | 2021-01-02 23:51:23 |
| 3        | 102         | 2        |            | NaN    | 2021-01-02 23:51:23 |
| 4        | 103         | 1        | 4          |        | 2021-01-04 13:23:46 |
| 4        | 103         | 1        | 4          |        | 2021-01-04 13:23:46 |
| 4        | 103         | 2        | 4          |        | 2021-01-04 13:23:46 |
| 5        | 104         | 1        | null       | 1      | 2021-01-08 21:00:29 |
| 6        | 101         | 2        | null       | null   | 2021-01-08 21:03:13 |
| 7        | 105         | 2        | null       | 1      | 2021-01-08 21:20:29 |
| 8        | 102         | 1        | null       | null   | 2021-01-09 23:54:33 |
| 9        | 103         | 1        | 4          | 1, 5   | 2021-01-10 11:22:59 |
| 10       | 104         | 1        | null       | null   | 2021-01-11 18:34:49 |
| 10       | 104         | 1        | 2, 6       | 1, 4   | 2021-01-11 18:34:49 |



**Table 3: runner_orders**

| order_id | runner_id | pickup_time         | distance | duration   | cancellation            |
| :------- | :-------- | :------------------ | :------- | :--------- | :---------------------- |
| 1        | 1         | 2021-01-01 18:15:34 | 20km     | 32 minutes |                         |
| 2        | 1         | 2021-01-01 19:10:54 | 20km     | 27 minutes |                         |
| 3        | 1         | 2021-01-03 00:12:37 | 13.4km   | 20 mins    | NaN                     |
| 4        | 2         | 2021-01-04 13:53:03 | 23.4     | 40         | NaN                     |
| 5        | 3         | 2021-01-08 21:10:57 | 10       | 15         | NaN                     |
| 6        | 3         | null                | null     | null       | Restaurant Cancellation |
| 7        | 2         | 2020-01-08 21:30:45 | 25km     | 25mins     | null                    |
| 8        | 2         | 2020-01-10 00:15:02 | 23.4 km  | 15 minute  | null                    |
| 9        | 2         | null                | null     | null       | Customer Cancellation   |
| 10       | 1         | 2020-01-11 18:50:20 | 10km     | 10minutes  | null                    |



**Table 4: pizza_names**

| pizza_id | pizza_name  |
| :------- | :---------- |
| 1        | Meat Lovers |
| 2        | Vegetarian  |



**Table 5: pizza_recipes**

| pizza_id | toppings                |
| -------- | ----------------------- |
| 1        | 1, 2, 3, 4, 5, 6, 8, 10 |
| 2        | 4, 6, 7, 9, 11, 12      |



**Table 6: pizza_toppings**

| topping_id | topping_name |
| :--------- | :----------- |
| 1          | Bacon        |
| 2          | BBQ Sauce    |
| 3          | Beef         |
| 4          | Cheese       |
| 5          | Chicken      |
| 6          | Mushrooms    |
| 7          | Onions       |
| 8          | Pepperoni    |
| 9          | Peppers      |
| 10         | Salami       |
| 11         | Tomatoes     |
| 12         | Tomato Sauce |


---
<a name="solution"></a>
## :boom: Questions and Solution
<a name="datacleaning"></a>
### **Data Cleaning**
 - ***New table: ##new_customer_orders***

```tsql

drop table if exists ##new_customer_orders
select row_number () over (order by order_id asc) as record_id,
       order_id, customer_id, pizza_id,
       case when exclusions = 'null' then '' else exclusions end as exclusions,
       case when extras = 'null' or extras is null then '' else extras end as extras,
       order_time
into ##new_customer_orders
from customer_orders;
```


|record_id|order_id|customer_id|pizza_id|exclusions|extras|order_time|
|---|---|---|---|---|---|---|
|1|1|101|1|||2020-01-01 18:05:02.000|
|2|2|101|1|||2020-01-01 19:00:52.000|
|3|3|102|1|||2020-01-02 23:51:23.000|
|4|3|102|2|||2020-01-02 23:51:23.000|
|5|4|103|1|4||2020-01-04 13:23:46.000|
|6|4|103|1|4||2020-01-04 13:23:46.000|
|7|4|103|2|4||2020-01-04 13:23:46.000|
|8|5|104|1||1|2020-01-08 21:00:29.000|
|9|6|101|2|||2020-01-08 21:03:13.000|
|10|7|105|2||1|2020-01-08 21:20:29.000|
|11|8|102|1|||2020-01-09 23:54:33.000|
|12|9|103|1|4|1, 5|2020-01-10 11:22:59.000|
|13|10|104|1|||2020-01-11 18:34:49.000|
|14|10|104|1|2, 6|1, 4|2020-01-11 18:34:49.000|

 - ***New table: ##new_runner_orders***

```tsql


drop table if exists ##new_runner_orders
select order_id, runner_id,
       case when pickup_time = 'null' then '' else pickup_time end as pickup_time,
       case when distance = 'null' then ''
            when distance like '%km' then trim ('km' from distance) 
            else distance
       end as distance,
       case when duration = 'null' then '' 
            when duration like '%nu%' then trim ('minutes' from duration)
            when duration like '%mi%' then trim ('mins' from duration)
            else duration
       end as duration,
       case when cancellation = 'null' or cancellation is null then  '' else cancellation end as cancellation
into ##new_runner_orders
from runner_orders;
```
|order_id|runner_id|pickup_time|distance|duration|cancellation|
|---|---|---|---|---|---|
|1|1|2020-01-01 18:15:34|20|32 ||
|2|1|2020-01-01 19:10:54|20|27 ||
|3|1|2020-01-03 00:12:37|13.4|20 ||
|4|2|2020-01-04 13:53:03|23.4|40||
|5|3|2020-01-08 21:10:57|10|15||
|6|3||||Restaurant Cancellation|
|7|2|2020-01-08 21:30:45|25|25||
|8|2|2020-01-10 00:15:02|23.4 |15 ||
|9|2||||Customer Cancellation|
|10|1|2020-01-11 18:50:20|10|10||

   - ***New table: ##new_pizza_recipes***
```tsql
drop table if exists ##new_pizza_recipes
select pizza_id, a.topping_id, topping_name
into ##new_pizza_recipes
from (select pizza_id, 
             cast (value as int) as topping_id
      from pizza_recipes r
      cross apply string_split (cast (toppings as varchar (50)),',')) a
join pizza_toppings t on t.topping_id = a.topping_id;
```

|pizza_id|topping_id|topping_name|
|---|---|---|
|1|1|Bacon|
|1|2|BBQ Sauce|
|1|3|Beef|
|1|4|Cheese|
|1|5|Chicken|
|1|6|Mushrooms|
|1|8|Pepperoni|
|1|10|Salami|
|2|4|Cheese|
|2|6|Mushrooms|
|2|7|Onions|
|2|9|Peppers|
|2|11|Tomatoes|
|2|12|Tomato Sauce|

   - ***New table: ##new_extras***

```tsql
drop table if exists ##new_extras
select row_number () over (order by order_id asc) as record_id,order_id,
       customer_id, pizza_id,
       cast (value as int) as extras
into ##new_extras
from ##new_customer_orders
cross apply string_split(cast (extras as varchar (50)),',')
where extras != '';
```

|record_id|order_id|customer_id|pizza_id|extras|
|---|---|---|---|---|
|1|5|104|1|1|
|2|7|105|2|1|
|3|9|103|1|1|
|4|9|103|1|5|
|5|10|104|1|1|
|6|10|104|1|4|

   - ***New table: ##new_exclusions***

```tsql
drop table if exists ##new_exclusions
select row_number () over (order by order_id asc) as record_id,order_id,
       customer_id, pizza_id,
       cast (value as int) as exclusions
into ##new_exclusions
from ##new_customer_orders
cross apply string_split(cast (exclusions as varchar (50)),',')
where exclusions != '';
```

|record_id|order_id|customer_id|pizza_id|exclusions|
|---|---|---|---|---|
|1|4|103|1|4|
|2|4|103|1|4|
|3|4|103|2|4|
|4|9|103|1|4|
|5|10|104|1|2|
|6|10|104|1|6|


---
<a name="metrics"></a>
### **A. Pizza Metrics**
 - **Question 1: How many pizzas were ordered?**

  ```tsql
select count (order_id) as count_order
from ##new_customer_orders;
```
|count_order|
|---|
|14|

 - **Question 2: How many unique customer orders were made?**

```tsql
select count (distinct order_id) as count_customer
from ##new_customer_orders;
```

|count_customer|
|---|
|10|

 - **Question 3: How many successful orders were delivered by each runner?**

```tsql
select runner_id,
       count (runner_id) as count_successful
from ##new_runner_orders
where cancellation = ''
group by runner_id;
```

|runner_id|count_successful|
|---|---|
|1|4|
|2|3|
|3|1|

 - **Question 4: How many of each type of pizza was delivered?**

```tsql
select cast (pizza_name as varchar (50)) as pizza_name,
       count (c.pizza_id) as count_pizza_was_delivered
from ##new_customer_orders c
join pizza_names p on p.pizza_id = c.pizza_id
join ##new_runner_orders r on r.order_id = c.order_id
where r.cancellation = ''
group by cast (pizza_name as varchar (50));
```

|pizza_name|count_pizza_was_delivered|
|---|---|
|Meatlovers|9|
|Vegetarian|3|


 - **Question 5: How many Vegetarian and Meatlovers were ordered by each customer?**

```tsql
select c.customer_id,
       cast (pizza_name as varchar (50)) as pizza_name,
       count (c.pizza_id) as count_pizza 
from ##new_customer_orders c
join pizza_names p on p.pizza_id = c.pizza_id
join ##new_runner_orders r on r.order_id = c.order_id
where r.cancellation = ''
group by cast (pizza_name as varchar (50)), c.customer_id
order by c.customer_id;
```

|customer_id|pizza_name|count_pizza|
|---|---|---|
|101|Meatlovers|2|
|102|Meatlovers|2|
|102|Vegetarian|1|
|103|Meatlovers|2|
|103|Vegetarian|1|
|104|Meatlovers|3|
|105|Vegetarian|1|

 - **Question 6: What was the maximum number of pizzas delivered in a single order?**

```tsql
with cte_count as (select c.order_id,
                          count (c.pizza_id) as count_pizza
                   from ##new_customer_orders c
                   join ##new_runner_orders r on r.order_id = c.order_id
                   where r.cancellation = ''
                   group by c.order_id)

select order_id,
       max (count_pizza) as max_pizza
from cte_count 
group by order_id;
```

|order_id|max_pizza|
|---|---|
|1|1|
|2|1|
|3|2|
|4|3|
|5|1|
|7|1|
|8|1|
|10|2|

 - **Question 7: For each customer, how many delivered pizzas had at least 1 change and how many had no changes?**

```tsql
with cte_status as (select order_id, customer_id, pizza_id,
                           case when extras != '' or exclusions !='' then 'change' else 'not change' end as 'status'
                    from ##new_customer_orders)

select customer_id, status,
       count (pizza_id) as count_pizza
from cte_status cte
join ##new_runner_orders r on r.order_id = cte.order_id
where r.cancellation = ''
group by customer_id, status
order by customer_id, status;
```

|customer_id|status|count_pizza|
|---|---|---|
|101|not change|2|
|102|not change|3|
|103|change|3|
|104|change|2|
|104|not change|1|
|105|change|1|

 - **Question 8: How many pizzas were delivered that had both exclusions and extras?**

```tsql
select count (c.pizza_id) as count_pizza
from ##new_customer_orders c
join ##new_runner_orders r on r.order_id = c.order_id
where r.cancellation = '' and exclusions != '' and extras != '';
```

|count_pizza|
|---|
|1|

 - **Question 9: What was the total volume of pizzas ordered for each hour of the day?**

```tsql
select datepart (hh,order_time) as hour,
       count (pizza_id) as count_pizza
from ##new_customer_orders c
join ##new_runner_orders r on r.order_id = c.order_id
where r.cancellation = ''
group by datepart (hh,order_time);
```
|hour|count_pizza|
|---|---|
|13|3|
|18|3|
|19|1|
|21|2|
|23|3|

 - **Question 10: What was the volume of orders for each day of the week?**

```tsql
select datename (dw,order_time) as [weekday],
       count (order_id) as count_pizza
from ##new_customer_orders
group by datename (dw,order_time);
```

|weekday|count_pizza|
|---|---|
|Friday|1|
|Saturday|5|
|Thursday|3|
|Wednesday|5|

---
<a name="experience"></a>
### **B. Runner and Customer Experience**
 - **Question 1: How many runners signed up for each 1 week period? (i.e. week starts 2021-01-01)**

```tsql
select datepart(ww,registration_date) as week,
       count (runner_id) as count_runners
from runners
group by datepart(ww,registration_date);
```

|week|count_runners|
|---|---|
|1|1|
|2|2|
|3|1|

 - **Question 2: What was the average time in minutes it took for each runner to arrive at the Pizza Runner HQ to pickup the order?**

```tsql
with cte_time as (select runner_id, r.order_id,
                         cast (datediff (minute, order_time, pickup_time) as float) as [time]
                  from ##new_runner_orders r
                  join ##new_customer_orders c on r.order_id = c.order_id
                  where cancellation =''
                  group by runner_id, cast (datediff (minute, order_time, pickup_time) as float), r.order_id)

select runner_id,
       round (avg([time]),2) as avg_time
from cte_time
group by runner_id;
```

|runner_id|avg_time|
|---|---|
|1|14.25|
|2|20.33|
|3|10|

 - **Question 3: Is there any relationship between the number of pizzas and how long the order takes to prepare?**

```tsql
with cte_time as (select c.order_id,
                         cast (datediff (minute, order_time, pickup_time) as float) as [time],
                         count(pizza_id) as count_pizza
                  from ##new_customer_orders c
                  join ##new_runner_orders r on r.order_id = c.order_id
                  where cancellation = ''
                  group by c.order_id, cast (datediff (minute, order_time, pickup_time) as float))

select count_pizza,
       round (avg ([time]),2) as avg_time,
       round ((avg ([time])/count_pizza),2) as avg_time_per_pizza
from cte_time 
group by count_pizza;
```

|count_pizza|avg_time|avg_time_per_pizza|
|---|---|---|
|1|12.2|12.2|
|2|18.5|9.25|
|3|30|10|

 - **Question 4: What was the average distance travelled for each customer?**

```tsql
select customer_id,
       round (avg(cast (distance as float)),2) as avg_distance
from ##new_runner_orders r
join ##new_customer_orders c on c.order_id = r.order_id
where cancellation =''
group by customer_id;
```

|customer_id|avg_distance|
|---|---|
|101|20|
|102|16.73|
|103|23.4|
|104|10|
|105|25|

 - **Question 5: What was the difference between the longest and shortest delivery times for all orders?**

```tsql
select max (cast (duration as float)) as longest,
       min (cast (duration as float)) as shortest,
       max (cast (duration as float)) - min (cast (duration as float)) as difference
from ##new_runner_orders
where cancellation ='';
```

|longest|shortest|difference|
|---|---|---|
|40|10|30|

 - **Question 6: What was the average speed for each runner for each delivery and do you notice any trend for these values?**

```tsql
with cte_avg as (select r.order_id, runner_id,
                        round (avg (cast (distance as float)/cast (duration as float))*60,2) as avg_speed
                 from ##new_runner_orders r
                 join ##new_customer_orders c on c.order_id = r.order_id
                 where cancellation = ''
                 group by runner_id, r.order_id)

select runner_id,
       avg (avg_speed) as avg_speed_by_runner
from cte_avg
group by runner_id;
```

|runner_id|avg_speed_by_runner|
|---|---|
|1|45.535|
|2|62.9|
|3|40|


 - **Question 7: What is the successful delivery percentage for each runner?**

```tsql
with cte_count as (select runner_id, 
                          count (order_id) as total_count,
                          case when cancellation = '' then count (order_id) else 0 end as count_successful
                   from ##new_runner_orders
                   group by runner_id, order_id, cancellation)

select runner_id,
       sum(count_successful)*100/sum(total_count) as successful_percentage
from cte_count
group by runner_id;
```

|runner_id|successful_percentage|
|---|---|
|1|100|
|2|75|
|3|50|

---
<a name="ingredient"></a>
### **C. Ingredient Optimisation**
 - **Question 1: What are the standard ingredients for each pizza?**

```tsql
select pizza_id, 
       string_agg (cast (topping_name as varchar (50)),',') as topping_name
from ##new_pizza_recipes
group by pizza_id;
```

|pizza_id|topping_name|
|---|---|
|1|Bacon,BBQ Sauce,Beef,Cheese,Chicken,Mushrooms,Pepperoni,Salami|
|2|Cheese,Mushrooms,Onions,Peppers,Tomatoes,Tomato Sauce|

 - **Question 2: What was the most commonly added extra?**

```tsql
select t.topping_id,
       cast (topping_name as varchar (50)) as topping_name,
       count (extras) as count_topping_added
from ##new_extras e
join pizza_toppings t on t.topping_id = e.extras
group by t.topping_id, cast (topping_name as varchar (50));
```

|topping_id|topping_name|count_topping_added|
|---|---|---|
|1|Bacon|4|
|4|Cheese|1|
|5|Chicken|1|

 - **Question 3: What was the most common exclusion?**

```tsql
select t.topping_id,
       cast (topping_name as varchar (50)) as topping_name,
       count (exclusions) as count_topping_exclusions
from ##new_exclusions ex
join pizza_toppings t on t.topping_id = ex.exclusions
group by t.topping_id, cast (topping_name as varchar (50));
```

|topping_id|topping_name|count_topping_exclusions|
|---|---|---|
|2|BBQ Sauce|1|
|4|Cheese|4|
|6|Mushrooms|1|

 - **Question 4: Generate an order item for each record in the customers_orders table in the format of one of the following:**
    - Meat Lovers
    - Meat Lovers - Exclude Beef
    - Meat Lovers - Extra Bacon
    - Meat Lovers - Exclude Cheese, Bacon - Extra Mushroom, Peppers



 - **Question 5: Generate an alphabetically ordered comma separated ingredient list for each pizza order from the customer_orders table and add a 2x in front of any relevant ingredients. For example: "Meat Lovers: 2xBacon, Beef, ... , Salami"**
 - **Question 6: What is the total quantity of each ingredient used in all delivered pizzas sorted by most frequent first?**

---
<a name="pricing"></a>
### **D. Pricing and Ratings**
 - **Question 1: If a Meat Lovers pizza costs $12 and Vegetarian costs $10 and there were no charges for changes - how much money has Pizza Runner made so far if there are no delivery fees?**
 - **Question 2: What if there was an additional $1 charge for any pizza extras?**
   - Add cheese is $1 extra
 - **Question 3: The Pizza Runner team now wants to add an additional ratings system that allows customers to rate their runner, how would you design an additional table for this new dataset - generate a schema for this new table and insert your own data for ratings for each successful customer order between 1 to 5.**
 - **Question 4: Using your newly generated table - can you join all of the information together to form a table which has the following information for successful deliveries?**
   - customer_id
   - order_id
   - runner_id
   - rating
   - order_time
   - pickup_time
   - Time between order and pickup
   - Delivery duration
   - Average speed
   - Total number of pizzas
 - **Question 5: If a Meat Lovers pizza was $12 and Vegetarian $10 fixed prices with no cost for extras and each runner is paid $0.30 per kilometre traveled - how much money does Pizza Runner have left over after these deliveries?**


---
<a name="bonus"></a>
### **E. Bonus Questions**
**If Danny wants to expand his range of pizzas - how would this impact the existing data design? Write an INSERT statement to demonstrate what would happen if a new Supreme pizza with all the toppings was added to the Pizza Runner menu?**















