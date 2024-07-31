# :tada: CASE STUDY #6 - CLIQUE BAIT

Find the full case study [**here**](https://8weeksqlchallenge.com/case-study-6/).

## :books: Table of Contents
 - [Introduction](#introduction)
 - [Entity Relationship Diagram](#entity)
 - [Example Datasets](#example)
 - [Questions and Solution](#solution)
   - [A. Digital Analysis](#digital)
   - [B. Product Funnel Analysis](#product)
   - [C. Campaigns Analysis](#campaigns)


---
<a name="introduction"></a>
## :question: Introduction

Clique Bait is not like your regular online seafood store - the founder and CEO Danny, was also a part of a digital data analytics team and wanted to expand his knowledge into the seafood industry!

In this case study - you are required to support Danny’s vision and analyse his dataset and come up with creative solutions to calculate funnel fallout rates for the Clique Bait online store.

---

<a name="entity"></a>
## :bookmark: Entity Relationship Diagram

---

<a name="example"></a>
## :open_book: Example Datasets

**Table 1: users**

| user_id | cookie_id | start_date          |
| :------ | :-------- | :------------------ |
| 397     | 3759ff    | 2020-03-30 00:00:00 |
| 215     | 863329    | 2020-01-26 00:00:00 |
| 191     | eefca9    | 2020-03-15 00:00:00 |
| 89      | 764796    | 2020-01-07 00:00:00 |
| 127     | 17ccc5    | 2020-01-22 00:00:00 |
| 81      | b0b666    | 2020-03-01 00:00:00 |
| 260     | a4f236    | 2020-01-08 00:00:00 |
| 203     | d1182f    | 2020-04-18 00:00:00 |
| 23      | 12dbc8    | 2020-01-18 00:00:00 |
| 375     | f61d69    | 2020-01-03 00:00:00 |



**Table 2: events**

| visit_id | cookie_id | page_id | event_type | sequence_number | event_time                 |
| :------- | :-------- | :------ | :--------- | :-------------- | :------------------------- |
| 719fd3   | 3d83d3    | 5       | 1          | 4               | 2020-03-02 00:29:09.975502 |
| fb1eb1   | c5ff25    | 5       | 2          | 8               | 2020-01-22 07:59:16.761931 |
| 23fe81   | 1e8c2d    | 10      | 1          | 9               | 2020-03-21 13:14:11.745667 |
| ad91aa   | 648115    | 6       | 1          | 3               | 2020-04-27 16:28:09.824606 |
| 5576d7   | ac418c    | 6       | 1          | 4               | 2020-01-18 04:55:10.149236 |
| 48308b   | c686c1    | 8       | 1          | 5               | 2020-01-29 06:10:38.702163 |
| 46b17d   | 78f9b3    | 7       | 1          | 12              | 2020-02-16 09:45:31.926407 |
| 9fd196   | ccf057    | 4       | 1          | 5               | 2020-02-14 08:29:12.922164 |
| edf853   | f85454    | 1       | 1          | 1               | 2020-02-22 12:59:07.652207 |
| 3c6716   | 02e74f    | 3       | 2          | 5               | 2020-01-31 17:56:20.777383 |



**Table 3: event_identifier**

| event_type | event_name    |
| :--------- | :------------ |
| 1          | Page View     |
| 2          | Add to Cart   |
| 3          | Purchase      |
| 4          | Ad Impression |
| 5          | Ad Click      |



**Table 4: campaign_identifier**

| campaign_id | products | campaign_name                     | start_date          | end_date            |
| :---------- | :------- | :-------------------------------- | :------------------ | :------------------ |
| 1           | 1-3      | BOGOF - Fishing For Compliments   | 2020-01-01 00:00:00 | 2020-01-14 00:00:00 |
| 2           | 4-5      | 25% Off - Living The Lux Life     | 2020-01-15 00:00:00 | 2020-01-28 00:00:00 |
| 3           | 6-8      | Half Off - Treat Your Shellf(ish) | 2020-02-01 00:00:00 | 2020-03-31 00:00:00 |



**Table 5: page_hierarchy**

| page_id | page_name      | product_category | product_id |
| :------ | :------------- | :--------------- | :--------- |
| 1       | Home Page      | null             | null       |
| 2       | All Products   | null             | null       |
| 3       | Salmon         | Fish             | 1          |
| 4       | Kingfish       | Fish             | 2          |
| 5       | Tuna           | Fish             | 3          |
| 6       | Russian Caviar | Luxury           | 4          |
| 7       | Black Truffle  | Luxury           | 5          |
| 8       | Abalone        | Shellfish        | 6          |
| 9       | Lobster        | Shellfish        | 7          |
| 10      | Crab           | Shellfish        | 8          |
| 11      | Oyster         | Shellfish        | 9          |
| 12      | Checkout       | null             | null       |
| 13      | Confirmation   | null             | null       |



---

<a name="solution"></a>
## :boom: Questions and Solution
<a name="digital"></a>
### **A. Digital Analysis**

**Using the available datasets - answer the following questions using a single query for each one:**

  - **Question 1: How many users are there?**

```tsql
select count (distinct user_id) as count_users
from [clique_bait.users];
```
|count_users|
|---|
|500|

  - **Question 2: How many cookies does each user have on average?**

```tsql
with cte_count_cookie as (select user_id,
                                 count (cookie_id) as count_cookie
                          from [clique_bait.users]
                          group by user_id)

select avg (count_cookie) as avg_cookie
from cte_count_cookie;
```
|avg_cookie|
|---|
|3|

  - **Question 3: What is the unique number of visits by all users per month?**

```tsql
select datepart (month, event_time) as [month],
       count (distinct visit_id) as count_visit
from [clique_bait.events]
group by datepart (month, event_time)
order by datepart (month, event_time);
```
|month|count_visit|
|---|---|
|1|876|
|2|1488|
|3|916|
|4|248|
|5|36|

  - **Question 4: What is the number of events for each event type?**

```tsql
select event_type,
       count (event_type) as count_events
from [clique_bait.events]
group by event_type
order by event_type;
```
|event_type|count_events|
|---|---|
|1|20928|
|2|8451|
|3|1777|
|4|876|
|5|702|

  - **Question 5: What is the percentage of visits which have a purchase event?**

```tsql
select 100 * count (distinct visit_id)/
       (select count (distinct visit_id) from [clique_bait.events]) as percentage_purchase
from [clique_bait.events] e
join [clique_bait.event_identifier] ei on ei.event_type = e.event_type
where event_name = 'purchase'
```
|percentage_purchase|
|---|
|49|

  - **Question 6: What is the percentage of visits which view the checkout page but do not have a purchase event?**

```tsql
select 100 * count (distinct visit_id)/
       (select count (distinct visit_id) from [clique_bait.events]) as percentage_purchase
from [clique_bait.events] e
join [clique_bait.event_identifier] ei on ei.event_type = e.event_type
join [clique_bait.page_hierarchy] h on h.page_id = e.page_id
where event_name != 'purchase' and page_name = 'checkout';
```
|percentage_purchase|
|---|
|59|

  - **Question 7: What are the top 3 pages by number of views?**

```tsql
select top (3) page_name,
               count (visit_id) as count_of_pageview
from [clique_bait.events] e
join [clique_bait.page_hierarchy] h on h.page_id = e.page_id
join [clique_bait.event_identifier] ei on ei.event_type = e.event_type
where event_name = 'pageview'
group by page_name
order by count (visit_id) desc;
```

|page_name|count_of_pageview|
|---|---|
|AllProducts|3174|
|Checkout|2103|
|HomePage|1782|

  - **Question 8: What is the number of views and cart adds for each product category?**

```tsql
select product_category,
       sum (case when event_name = 'Pageview' then 1 else 0 end) as count_pageview,
       sum (case when event_name = 'Addtocart' then 1 else 0 end) as count_addtocart
from [clique_bait.events] e
join [clique_bait.page_hierarchy] h on h.page_id = e.page_id
join [clique_bait.event_identifier] ei on ei.event_type = e.event_type
group by product_category
order by product_category;
```
|product_category|count_pageview|count_addtocart|
|---|---|---|
|NULL|7059|0|
|Fish|4633|2789|
|Luxury|3032|1870|
|Shellfish|6204|3792|

  - **Question 9: What are the top 3 products by purchases?**

```tsql
with cte_purchase as (select visit_id, e.page_id, product_category, event_name
                      from [clique_bait.events] e
                      join [clique_bait.page_hierarchy] h on h.page_id = e.page_id
                      join [clique_bait.event_identifier] ei on ei.event_type = e.event_type
                      where visit_id in (select visit_id
                                          from [clique_bait.events]
                                          where event_type = 3)) -- purchase

select top (3) page_name,
               count (cte.page_id) as count_product
from cte_purchase cte
join [clique_bait.page_hierarchy] h on h.page_id = cte.page_id
where cte.product_category is not null and event_name = 'addtocart' --addtocart to purchase
group by page_name
order by count (cte.page_id) desc;
```

|page_name|count_product|
|---|---|
|Lobster|754|
|Oyster|726|
|Crab|719|

---
<a name="product"></a>
### **B. Product Funnel Analysis**

 - **Using a single SQL query - create a new output table which has the following details:**
   - **How many times was each product viewed?**
   - **How many times was each product added to cart?**
   - **How many times was each product added to a cart but not purchased (abandoned)?**
   - **How many times was each product purchased?**

***Query*** 
```tsql
drop table if exists ##product;
with cte_count_view_add as (select page_name, product_category,
                                   sum (case when event_type = 1 then 1 else 0 end) as views,
                                   sum (case when event_type = 2 then 1 else 0 end) as addtocard
                            from [clique_bait.events] e
                            join [clique_bait.page_hierarchy] h on h.page_id = e.page_id
                            group by page_name, product_category),

     cte_count_abandoned as (select page_name, product_category,
                                           count (visit_id) as abandoned
                                    from [clique_bait.events] e
                                    join [clique_bait.page_hierarchy] h on e.page_id = h.page_id
                                    where event_type = 2 and visit_id not in (select visit_id
                                                                              from [clique_bait.events]
                                                                              where event_type = 3)
                                    group by page_name, product_category),

     cte_count_purchase as (select page_name, product_category,
                                   count (visit_id) as purchase
                            from [clique_bait.events] e
                            join [clique_bait.page_hierarchy] h on e.page_id = h.page_id
                            where event_type = 2 and visit_id in (select visit_id
                                                                  from [clique_bait.events]
                                                                  where event_type = 3)
                            group by page_name, product_category)

select va.page_name as product_name, va.product_category, va.views, va.addtocard, ab.abandoned, p.purchase
into ##product
from cte_count_view_add va
join cte_count_abandoned ab on va.page_name = ab.page_name
join cte_count_purchase p on va.page_name = p.page_name
order by va.page_name;
```
***New table: ##Product***
|product_name|product_category|views|addtocard|abandoned|purchase|
|---|---|---|---|---|---|
|Abalone|Shellfish|1525|932|233|699|
|BlackTruffle|Luxury|1469|924|217|707|
|Crab|Shellfish|1564|949|230|719|
|Kingfish|Fish|1559|920|213|707|
|Lobster|Shellfish|1547|968|214|754|
|Oyster|Shellfish|1568|943|217|726|
|RussianCaviar|Luxury|1563|946|249|697|
|Salmon|Fish|1559|938|227|711|
|Tuna|Fish|1515|931|234|697|

 - **Additionally, create another table which further aggregates the data for the above points but this time for each product category instead of individual products.**

***Query***
```tsql
drop table if exists ##product_category;
with cte_count_view_add as (select product_category,
                                   sum (case when event_type = 1 then 1 else 0 end) as views,
                                   sum (case when event_type = 2 then 1 else 0 end) as addtocard
                            from [clique_bait.events] e
                            join [clique_bait.page_hierarchy] h on h.page_id = e.page_id
                            group by product_category),

     cte_count_abandoned as (select product_category,
                                           count (visit_id) as abandoned
                             from [clique_bait.events] e
                             join [clique_bait.page_hierarchy] h on e.page_id = h.page_id
                             where event_type = 2 and visit_id not in (select visit_id
                                                                       from [clique_bait.events]
                                                                       where event_type = 3)
                             group by product_category),

     cte_count_purchase as (select product_category,
                                   count (visit_id) as purchase
                            from [clique_bait.events] e
                            join [clique_bait.page_hierarchy] h on e.page_id = h.page_id
                            where event_type = 2 and visit_id in (select visit_id
                                                                  from [clique_bait.events]
                                                                  where event_type = 3)
                            group by product_category)

select va.product_category, va.views, va.addtocard, ab.abandoned, p.purchase
into ##product_category
from cte_count_view_add va
join cte_count_abandoned ab on va.product_category = ab.product_category
join cte_count_purchase p on va.product_category = p.product_category
order by va.product_category;
```
***New table: ##product_category***

|product_category|views|addtocard|abandoned|purchase|
|---|---|---|---|---|
|Fish|4633|2789|674|2115|
|Luxury|3032|1870|466|1404|
|Shellfish|6204|3792|894|2898|

 - **Use your 2 new output tables - answer the following questions:**
   - **Question 1:Which product had the most views, cart adds and purchases?**
  
```tsql
with cte_rank as (select product_name,
                         row_number () over (order by views desc) as rank_views,
                         row_number () over (order by addtocard desc) as rank_add,
                         row_number () over (order by purchase desc) as rank_purchase
                  from ##product)

select max (case when rank_views = 1 then product_name end) as product_most_views,
       max (case when rank_add = 1 then product_name end) as product_most_add,
       max (case when rank_purchase = 1 then product_name end) as product_most_purchase
from cte_rank;
```
|product_most_views|product_most_add|product_most_purchase|
|---|---|---|
|Oyster|Lobster|Lobster|

   - **Question 2:Which product was most likely to be abandoned?**

```tsql
with cte_rank as (select product_name,
                         round (100* purchase / addtocard,2) as percentage_abandoned,
                         dense_rank () over (order by round (100* purchase / addtocard,2)) as rank_percentage_abandoned
                  from ##product)

select *
from cte_rank
where rank_percentage_abandoned = 1;
```
|product_name|percentage_abandoned|rank_percentage_abandoned|
|---|---|---|
|RussianCaviar|73|1|

   - **Question 3:Which product had the highest view to purchase percentage?**

```tsql
with cte_rank_percentage as (select  product_name,
                                     round (100 * purchase / views, 2) as [percentage],
                                     dense_rank () over (order by round (100 * purchase / views, 2) desc )as [rank_percentage]
                             from ##product)

select *
from cte_rank_percentage
where rank_percentage = 1;
```

|product_name|percentage|rank_percentage|
|---|---|---|
|BlackTruffle|48|1|
|Lobster|48|1|

   - **Question 4:What is the average conversion rate from view to cart add?**

```tsql
select round (avg (100* addtocard / views),2) as avg_add_from_views
from ##product;
```

|avg_add_from_views|
|---|
|60|

   - **Question 5:What is the average conversion rate from cart add to purchase?**

```tsql
select round (avg (100* purchase / addtocard),2) as avg_add_from_views
from ##product;
```
|avg_add_from_views|
|---|
|75|


---
<a name="campaigns"></a>
### **C. Campaigns Analysis**

 - **Generate a table that has 1 single row for every unique visit_id record and has the following columns:**
   - **user_id**
   - **visit_id**
   - **visit_start_time: the earliest event_time for each visit**
   - **page_views: count of page views for each visit**
   - **cart_adds: count of product cart add events for each visit**
   - **purchase: 1/0 flag if a purchase event exists for each visit**
   - **campaign_name: map the visit to a campaign if the visit_start_time falls between the start_date and end_date**
   - **impression: count of ad impressions for each visit**
   - **click: count of ad clicks for each visit**
   - **(Optional column) cart_products: a comma separated text value with products added to the cart sorted by the order they were added to the cart (hint: use the sequence_number)**

```tsql
with cte_event_type as (select user_id,visit_id,
                               min (event_time) as visit_start_time,
                               sum (case when event_type = 1 then 1 else 0 end) as page_views,
                               sum (case when event_type = 2 then 1 else 0 end) as cart_adds,
                               max (case when event_type = 3 then 1 else 0 end) as purchase,
                               sum (case when event_type = 4 then 1 else 0 end) as impression,
                               sum (case when event_type = 5 then 1 else 0 end) as click
                        from [clique_bait.users] u
                        join [clique_bait.events] e on u.cookie_id = e.cookie_id
                        group by user_id, visit_id),

     cte_stringagg_product as (select e.visit_id,
                                      string_agg (page_name,',') within group (order by e.sequence_number)as cart_products
                               from [clique_bait.events] e
                               join [clique_bait.page_hierarchy] h on h.page_id = e.page_id
                               where product_id is not null and event_type = 2
                               group by e.visit_id)

select user_id,cte.visit_id,visit_start_time,page_views,cart_adds,purchase, campaign_name,impression,click, 
       case when cart_adds = 0 then '' else cart_products end as cart_products
from cte_event_type cte
left join cte_stringagg_product cte1 on cte1.visit_id = cte.visit_id
join [clique_bait.campaign_identifier] c on cte.visit_start_time between c.start_date and c.end_date
order by user_id,cte.visit_id;
```
|user_id|visit_id|visit_start_time|page_views|cart_adds|purchase|campaign_name|impression|click|cart_products|
|---|---|---|---|---|---|---|---|---|---|
|1|02a5d5|2020-02-26 16:57:26.260|4|0|0|HalfOff-TreatYourShellfish|0|0||
|1|0826dc|2020-02-26 05:58:37.920|1|0|0|HalfOff-TreatYourShellfish|0|0||
|1|0fc437|2020-02-04 17:49:49.603|10|6|1|HalfOff-TreatYourShellfish|1|1|Tuna,RussianCaviar,BlackTruffle,Abalone,Crab,Oyster|
|1|30b94d|2020-03-15 13:12:54.023|9|7|1|HalfOff-TreatYourShellfish|1|1|Salmon,Kingfish,Tuna,RussianCaviar,Abalone,Lobster,Crab|
|1|41355d|2020-03-25 00:11:17.860|6|1|0|HalfOff-TreatYourShellfish|0|0|Lobster|
|1|ccf365|2020-02-04 19:16:09.183|7|3|1|HalfOff-TreatYourShellfish|0|0|Lobster,Crab,Oyster|
|1|eaffde|2020-03-25 20:06:32.343|10|8|1|HalfOff-TreatYourShellfish|1|1|Salmon,Tuna,RussianCaviar,BlackTruffle,Abalone,Lobster,Crab,Oyster|
|1|f7c798|2020-03-15 02:23:26.313|9|3|1|HalfOff-TreatYourShellfish|0|0|RussianCaviar,Crab,Oyster|
|2|0635fb|2020-02-16 06:42:42.737|9|4|1|HalfOff-TreatYourShellfish|0|0|Salmon,Kingfish,Abalone,Crab|
|2|1f1198|2020-02-01 21:51:55.080|1|0|0|HalfOff-TreatYourShellfish|0|0||

***The result has 3052 rows***



 - **Use the subsequent dataset to generate at least 5 insights for the Clique Bait team - bonus: prepare a single A4 infographic that the team can use for their management reporting sessions, be sure to emphasise the most important points from your findings.**

 - **Some ideas you might want to investigate further include:**

   - **Identifying users who have received impressions during each campaign period and comparing each metric with other users who did not have an impression event**
   - **Does clicking on an impression lead to higher purchase rates?**
   - **What is the uplift in purchase rate when comparing users who click on a campaign impression versus users who do not receive an impression? What if we compare them with users who just an impression but do not click?**
   - **What metrics can you use to quantify the success or failure of each campaign compared to eachother?**















