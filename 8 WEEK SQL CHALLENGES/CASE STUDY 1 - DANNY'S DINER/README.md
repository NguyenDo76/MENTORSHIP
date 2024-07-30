# :tada: CASE STUDY #1 - DANNY'S DINER

Find the full case study [**here**](https://8weeksqlchallenge.com/case-study-1/).

## :books: Table of Contents
 - [Introduction](#introduction)
 - [Entity Relationship Diagram](#entity)
 - [Example Datasets](#example)
 - [Questions and Solution](#solution)

<a name="introduction"></a>
## :question: Introduction

Danny seriously loves Japanese food so in the beginning of 2021, he decides to embark upon a risky venture and opens up a cute little restaurant that sells his 3 favourite foods: sushi, curry and ramen.

Danny’s Diner is in need of your assistance to help the restaurant stay afloat - the restaurant has captured some very basic data from their few months of operation but have no idea how to use their data to help them run the business.

<a name="entity"></a>
## :bookmark: Entity Relationship Diagram

<a name="example"></a>
## :open_book: Example Datasets


  
**Table 1: Sales**
|customer_id|order_date|product_id|
|---|---|---|
|A|2021-01-01|1|
|A|2021-01-01|2|
|A|2021-01-07|2|
|A|2021-01-10|3|
|A|2021-01-11|3|
|A|2021-01-11|3|
|B|2021-01-01|2|
|B|2021-01-02|2|
|B|2021-01-04|1|
|B|2021-01-11|1|
|B|2021-01-16|3|
|B|2021-02-01|3|
|C|2021-01-01|3|
|C|2021-01-01|3|
|C|2021-01-07|3|


<br>


  
**Table 2: Menu**

|product_id|product_name|price|
|---|---|---|
|1|sushi|10|
|2|curry|15|
|3|ramen|12|

<br>



**Table 3: Members**

|customer_id|join_date|
|---|---|
|A|2021-01-07|
|B|2021-01-09|

</div>


<a name="solution"></a>
## :boom: Questions and Solution

### **Question 1: What is the total amount each customer spent at the restaurant?**

```tsql

select s.customer_id,
       sum (m.price) as total_price
from sales s
join menu m on m.product_id = s.product_id
group by s.customer_id;
```
|customer_id|total_price|
|---|---|
|A|76|
|B|74|
|C|36|

### **Question 2: How many days has each customer visited the restaurant?**

```tsql

select customer_id,
       count (distinct order_date) as count_day
From sales
group by customer_id;
```

|customer_id|count_day|
|---|---|
|A|4|
|B|6|
|C|2|

### **Question 3: What was the first item from the menu purchased by each customer?**

```tsql

with cte_rank as (select s.customer_id, m.product_name,
                         dense_rank () over (partition by s.customer_id order by s.order_date asc) as "rank_date"
                  from sales s
                  join menu m on m.product_id = s.product_id)

select customer_id, product_name
from cte_rank
where rank_date = 1
group by customer_id, product_name;
```

|customer_id|product_name|
|---|---|
|A|curry|
|A|sushi|
|B|curry|
|C|ramen|

### **Question 4: What is the most purchased item on the menu and how many times was it purchased by all customers?**

```tsql

select top (1) m.product_name,
               count (s.product_id) as count_product
from sales s
join menu m on s.product_id = m.product_id
group by m.product_name;
```

|product_name|count_product|
|---|---|
|curry|4|

### **Question 5: Which item was the most popular for each customer?**

```tsql

with cte_rank as (select customer_id, m.product_name, count(s.product_id) as count_product,
                         dense_rank () over (partition by customer_id order by COUNT(s.product_id) desc) as rank_product
                  from sales s
                  join menu m on s.product_id = m.product_id
                  group by customer_id, m.product_name)

select customer_id, product_name, count_product
from cte_rank
where rank_product = 1;
```

|customer_id|product_name|count_product|
|---|---|---|
|A|ramen|3|
|B|sushi|2|
|B|curry|2|
|B|ramen|2|
|C|ramen|3|

### **Question 6: Which item was purchased first by the customer after they became a member?**

***Method 1:***
```tsql

with cte_days as (select s.customer_id, s.product_id,
                         datediff (DY,mb.join_date,s.order_date) as days_difference
                  from sales s
                  join members mb on mb.customer_id = s.customer_id),

     cte_rank as (select customer_id, product_id,
                         row_number () over (partition by customer_id order by days_difference asc) as 'rank'
                  from cte_days
                  where days_difference > 0)

SELECT customer_id, cte.product_id, product_name
from cte_rank cte
join menu m on cte.product_id = m.product_id
where rank = 1;
```
|customer_id|product_id|product_name|
|---|---|---|
|A|3|ramen|
|B|1|sushi|

***Method 2:***

```tsql

with cte_rank as (select s.customer_id, product_id,
                         row_number () over (partition by s.customer_id order by s.order_date asc) as 'rank'
                  from sales s
                  join members mb on mb.customer_id = s.customer_id
                  where mb.join_date < s.order_date)

SELECT customer_id, cte.product_id, product_name
from cte_rank cte
join menu m on cte.product_id = m.product_id
where rank = 1;
````
|customer_id|product_id|product_name|
|---|---|---|
|A|3|ramen|
|B|1|sushi|


### **Question 7: Which item was purchased just before the customer became a member?**

***Method 1:***

```tsql

with cte_days as (select s.customer_id, s.product_id,
                         datediff (DY,mb.join_date,s.order_date) as days_difference
                  from sales s
                  join members mb on mb.customer_id = s.customer_id),

    cte_rank as (select customer_id, product_id,
                        row_number () over (partition by customer_id order by days_difference desc) as 'rank'
                 from cte_days
                 where days_difference < 0)

SELECT customer_id, cte.product_id, product_name
from cte_rank cte
join menu m on cte.product_id = m.product_id
where rank = 1;
```

|customer_id|product_id|product_name|
|---|---|---|
|A|1|sushi|
|B|1|sushi|


***Method 2:***

```tsql

with cte_rank as (select s.customer_id, product_id,
                         row_number () over (partition by s.customer_id order by s.order_date desc) as 'rank'
                  from sales s
                  join members mb on mb.customer_id = s.customer_id
                  where mb.join_date > s.order_date)

SELECT customer_id, cte.product_id, product_name
from cte_rank cte
join menu m on cte.product_id = m.product_id
where rank = 1;
```
|customer_id|product_id|product_name|
|---|---|---|
|A|1|sushi|
|B|1|sushi|

### **Question 8: What is the total items and amount spent for each member before they became a member?**

```tsql

select s.customer_id, 
       count (s.product_id) as total_items,
       sum (price) as total_price
from sales s 
join members mb on mb.customer_id = s.customer_id
join menu m on m.product_id = s.product_id
where mb.join_date > s.order_date
group by s.customer_id;
```

|customer_id|total_items|total_price|
|---|---|---|
|A|2|25|
|B|3|40|


### **Question 9: If each $1 spent equates to 10 points and sushi has a 2x points multiplier - how many points would each customer have?**

```tsql
with cte_points as (select s.customer_id,
                           case when m.product_name = 'sushi' then m.price * 10 * 2 else m.price *10 end as total_points
                    from sales s 
                    join menu m on m.product_id = s.product_id)

select customer_id,
       sum (total_points) as total_point_by_customer
from cte_points
group by customer_id;
```

|customer_id|total_point_by_customer|
|---|---|
|A|860|
|B|940|
|C|360|

### **Question 10: In the first week after a customer joins the program (including their join date) they earn 2x points on all items, not just sushi - how many points do customer A and B have at the end of January?**

***REF 1:***

```tsql
with cte_points as (select s.customer_id,
                           case when m.product_name = 'sushi' then m.price * 10 * 2
                                when DATEDIFF (DY,mb.join_date,s.order_date) BETWEEN 0 and 6 then price *10 * 2
                                else price *10
                           end as total_points
                    from sales s
                    join menu m on m.product_id = s.product_id
                    join members mb on mb.customer_id = s.customer_id
                    where MONTH(s.order_date) = 1)

select customer_id, 
       sum (total_points) as total_point_by_customer_in_January
from cte_points 
group by customer_id;
```

|customer_id|total_point_by_customer_in_January|
|---|---|
|A|1370|
|B|820|

***REF 2:***

```tsql
with cte_points as (select s.customer_id,s.order_date,mb.join_date,
                           case when m.product_name = 'sushi' then m.price * 10 * 2
                                when datediff (DY,mb.join_date,s.order_date) BETWEEN 0 and 6 then price *10 * 2
                                else price *10
                           end as total_points
                    from sales s
                    join menu m on m.product_id = s.product_id
                    join members mb on mb.customer_id = s.customer_id
                    where MONTH(s.order_date) = 1 and mb.join_date <= s.order_date)

select customer_id, 
       sum (total_points) as total_point_by_customer_in_January
from cte_points
group by customer_id;
```

|customer_id|total_point_by_customer_in_January|
|---|---|
|A|1020|
|B|320|


### **Bonus question: Join All The Things**
The following questions are related creating basic data tables that Danny and his team can use to quickly derive insights without needing to join the underlying tables using SQL.
Recreate the following table output using the available data:

```tsql
select s.customer_id, s.order_date, m.product_name, m.price,
       case when datediff (dy,mb.join_date,s.order_date) >= 0 then 'Y' else 'N' end as member
from sales s
left join menu m on m.product_id = s.product_id
left join members mb on mb.customer_id = s.customer_id
```

|customer_id|order_date|product_name|price|member|
|---|---|---|---|---|
|A|2021-01-01|sushi|10|N|
|A|2021-01-01|curry|15|N|
|A|2021-01-07|curry|15|Y|
|A|2021-01-10|ramen|12|Y|
|A|2021-01-11|ramen|12|Y|
|A|2021-01-11|ramen|12|Y|
|B|2021-01-01|curry|15|N|
|B|2021-01-02|curry|15|N|
|B|2021-01-04|sushi|10|N|
|B|2021-01-11|sushi|10|Y|
|B|2021-01-16|ramen|12|Y|
|B|2021-02-01|ramen|12|Y|
|C|2021-01-01|ramen|12|N|
|C|2021-01-01|ramen|12|N|
|C|2021-01-07|ramen|12|N|

### **Rank All The Things**
Danny also requires further information about the ranking of customer products, but he purposely does not need the ranking for non-member purchases so he expects null ranking values for the records when customers are not yet part of the loyalty program.

```tsql
with cte_members as (select s.customer_id, s.order_date, m.product_name, m.price,
                            case when datediff (dy,mb.join_date,s.order_date) >=  0 then 'Y' else 'N' end as member
                     from sales s
                     left join menu m on m.product_id = s.product_id
                     left join members mb on mb.customer_id = s.customer_id)

select *,
       case when member = 'Y' then dense_rank () over (partition by customer_id, member order by order_date asc) end as ranking
from cte_members
```

|customer_id|order_date|product_name|price|member|ranking|
|---|---|---|---|---|---|
|A|2021-01-01|sushi|10|N|NULL|
|A|2021-01-01|curry|15|N|NULL|
|A|2021-01-07|curry|15|Y|1|
|A|2021-01-10|ramen|12|Y|2|
|A|2021-01-11|ramen|12|Y|3|
|A|2021-01-11|ramen|12|Y|3|
|B|2021-01-01|curry|15|N|NULL|
|B|2021-01-02|curry|15|N|NULL|
|B|2021-01-04|sushi|10|N|NULL|
|B|2021-01-11|sushi|10|Y|1|
|B|2021-01-16|ramen|12|Y|2|
|B|2021-02-01|ramen|12|Y|3|
|C|2021-01-01|ramen|12|N|NULL|
|C|2021-01-01|ramen|12|N|NULL|
|C|2021-01-07|ramen|12|N|NULL|








