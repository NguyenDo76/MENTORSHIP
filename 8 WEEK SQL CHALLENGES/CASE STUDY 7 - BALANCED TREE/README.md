# :tada: CASE STUDY #7 - BALANCED TREE

Find the full case study [**here**](https://8weeksqlchallenge.com/case-study-7/).

## :books: Table of Contents
 - [Introduction](#introduction)
 - [Entity Relationship Diagram](#entity)
 - [Example Datasets](#example)
 - [Questions and Solution](#solution)
   - [A. High Level Sales Analysi](#high)
   - [B. Transaction Analysis](#transaction)
   - [C. Product Analysis](#product)
   - [D. Reporting Challenge](#report)
   - [E. Bonus Challenge](#bonus)


---
<a name="introduction"></a>
## :question: Introduction
Balanced Tree Clothing Company prides themselves on providing an optimised range of clothing and lifestyle wear for the modern adventurer!

Danny, the CEO of this trendy fashion company has asked you to assist the team’s merchandising teams analyse their sales performance and generate a basic financial report to share with the wider business.

---

<a name="entity"></a>
## :bookmark: Entity Relationship Diagram

<img src="https://github.com/NguyenDo76/MENTORSHIP/blob/main/8%20WEEK%20SQL%20CHALLENGES/IMAGE/CASE%20STUDY%207%20-%20BALANCED%20TREE.png">

---

<a name="example"></a>
## :open_book: Example Datasets

**Table 1: product_details**

| product_id | price | product_name                     | category_id | segment_id | style_id | category_name | segment_name | style_name          |
| :--------- | :---- | :------------------------------- | :---------- | :--------- | :------- | :------------ | :----------- | :------------------ |
| c4a632     | 13    | Navy Oversized Jeans - Womens    | 1           | 3          | 7        | Womens        | Jeans        | Navy Oversized      |
| e83aa3     | 32    | Black Straight Jeans - Womens    | 1           | 3          | 8        | Womens        | Jeans        | Black Straight      |
| e31d39     | 10    | Cream Relaxed Jeans - Womens     | 1           | 3          | 9        | Womens        | Jeans        | Cream Relaxed       |
| d5e9a6     | 23    | Khaki Suit Jacket - Womens       | 1           | 4          | 10       | Womens        | Jacket       | Khaki Suit          |
| 72f5d4     | 19    | Indigo Rain Jacket - Womens      | 1           | 4          | 11       | Womens        | Jacket       | Indigo Rain         |
| 9ec847     | 54    | Grey Fashion Jacket - Womens     | 1           | 4          | 12       | Womens        | Jacket       | Grey Fashion        |
| 5d267b     | 40    | White Tee Shirt - Mens           | 2           | 5          | 13       | Mens          | Shirt        | White Tee           |
| c8d436     | 10    | Teal Button Up Shirt - Mens      | 2           | 5          | 14       | Mens          | Shirt        | Teal Button Up      |
| 2a2353     | 57    | Blue Polo Shirt - Mens           | 2           | 5          | 15       | Mens          | Shirt        | Blue Polo           |
| f084eb     | 36    | Navy Solid Socks - Mens          | 2           | 6          | 16       | Mens          | Socks        | Navy Solid          |
| b9a74d     | 17    | White Striped Socks - Mens       | 2           | 6          | 17       | Mens          | Socks        | White Striped       |
| 2feb6b     | 29    | Pink Fluro Polkadot Socks - Mens | 2           | 6          | 18       | Mens          | Socks        | Pink Fluro Polkadot |



**Table 2: sales**

| prod_id | qty  | price | discount | member | txn_id | start_txn_time           |
| :------ | :--- | :---- | :------- | :----- | :----- | :----------------------- |
| c4a632  | 4    | 13    | 17       | t      | 54f307 | 2021-02-13 01:59:43.296  |
| 5d267b  | 4    | 40    | 17       | t      | 54f307 | 2021-02-13 01:59:43.296  |
| b9a74d  | 4    | 17    | 17       | t      | 54f307 | 2021-02-13 01:59:43.296  |
| 2feb6b  | 2    | 29    | 17       | t      | 54f307 | 2021-02-13 01:59:43.296  |
| c4a632  | 5    | 13    | 21       | t      | 26cc98 | 2021-01-19 01:39:00.3456 |
| e31d39  | 2    | 10    | 21       | t      | 26cc98 | 2021-01-19 01:39:00.3456 |
| 72f5d4  | 3    | 19    | 21       | t      | 26cc98 | 2021-01-19 01:39:00.3456 |
| 2a2353  | 3    | 57    | 21       | t      | 26cc98 | 2021-01-19 01:39:00.3456 |
| f084eb  | 3    | 36    | 21       | t      | 26cc98 | 2021-01-19 01:39:00.3456 |
| c4a632  | 1    | 13    | 21       | f      | ef648d | 2021-01-27 02:18:17.1648 |




**Table 3: product_hierarcy**

|id|parent_id|level_text|level_name|
|---|---|---|---|
|1|NULL|Womens|Category|
|2|NULL|Mens|Category|
|3|1|Jeans|Segment|
|4|1|Jacket|Segment|
|5|2|Shirt|Segment|
|6|2|Socks|Segment|
|7|3|NavyOversized|Style|
|8|3|BlackStraight|Style|
|9|3|CreamRelaxed|Style|
|10|4|KhakiSuit|Style|
|11|4|IndigoRain|Style|
|12|4|GreyFashion|Style|
|13|5|WhiteTee|Style|
|14|5|TealButtonUp|Style|
|15|5|BluePolo|Style|
|16|6|NavySolid|Style|
|17|6|WhiteStriped|Style|
|18|6|PinkFluroPolkadot|Style|


**Table 4: product_prices**

|id|product_id|price|
|---|---|---|
|7|c4a632|13|
|8|e83aa3|32|
|9|e31d39|10|
|10|d5e9a6|23|
|11|72f5d4|19|
|12|9ec847|54|
|13|5d267b|40|
|14|c8d436|10|
|15|2a2353|57|
|16|f084eb|36|
|17|b9a74d|17|
|18|2feb6b|29|

---

<a name="solution"></a>
## :boom: Questions and Solution
<a name="high"></a>
### **A. High Level Sales Analysis**

 - **Question 1: What was the total quantity sold for all products?**

```tsql
select sum (qty) as total_product
from [balanced_tree.sales];
```

|total_product|
|---|
|45216|

 - **Question 2: What is the total generated revenue for all products before discounts?**

```tsql
select sum (qty* price) as revenue_before_discount
from [balanced_tree.sales];
```
|revenue_before_discount|
|---|
|1289453|

 - **Question 3: What was the total discount amount for all products?**

```tsql
select sum (qty * price * discount/100) as total_discount
from [balanced_tree.sales];
```

|total_discount|
|---|
|149486|


---
<a name="transaction"></a>
### **B. Transaction Analysis**

 - **Question 1: How many unique transactions were there?**

```tsql
select count (distinct txn_id) as count_unique_trans 
from [balanced_tree.sales];
```

|count_unique_trans|
|---|
|2500|

 - **Question 2: What is the average unique products purchased in each transaction?**

```tsql
with cte_prod as (select txn_id,
                         count (prod_id) as count_prod
                  from [balanced_tree.sales]
                  group by txn_id)

select avg(count_prod) as avg_prod
from cte_prod;
```

|avg_prod|
|---|
|6|

 - **Question 3: What are the 25th, 50th and 75th percentile values for the revenue per transaction?**

```tsql
with cte_revenue as (select txn_id,
                            sum (qty * price) as revenue
                     from [balanced_tree.sales]
                     group by txn_id)

select distinct percentile_cont (0.25) within group (order by revenue) over () as P25,
                percentile_cont (0.5) within group (order by revenue) over () as median,
                percentile_cont (0.75) within group (order by revenue) over () as P75
from cte_revenue;
```

|P25|median|P75|
|---|---|---|
|375.75|509.5|647|

 - **Question 4: What is the average discount value per transaction?**

```tsql
with cte_discount as (select txn_id,
                             sum (qty * price * discount/100) as total_discount
                      from [balanced_tree.sales]
                      group by txn_id)

select avg (total_discount) as avg_discount
from cte_discount;
```

|avg_discount|
|---|
|59|

 - **Question 5: What is the percentage split of all transactions for members vs non-members?**

```tsql
with cte_member as (select distinct txn_id,
                                    case when member = 't' then 'member' else 'non-member' end as member
                    from [balanced_tree.sales])

select member,
       count (member) as count_member,
       cast (round (count (member) * 100.0 / sum (count (member)) over (),2) as float) as percentage_member
from cte_member
group by member;
```

|member|count_member|percentage_member|
|---|---|---|
|member|1505|60.2|
|non-member|995|39.8|

 - **Question 6: What is the average revenue for member transactions and non-member transactions?**

```tsql
with cte_member as (select txn_id,
                           case when member = 't' then 'member' else 'non-member' end as member,
                           sum (qty * price) as revenue
                    from [balanced_tree.sales]
                    group by txn_id, member)

select member,
       avg (revenue) as avg_revenue
from cte_member
group by member;
```

|member|avg_revenue|
|---|---|
|member|516|
|non-member|515|


---
<a name="product"></a>
### **C. Product Analysis**

 - **Question 1: What are the top 3 products by total revenue before discount?**

```tsql
select top (3) d.product_name,
               sum (qty * s.price) as total_revenue
from [balanced_tree.sales] s
join [balanced_tree.product_details] d on s.prod_id = d.product_id
group by d.product_name
order by sum (qty * s.price) desc;
```

|product_name|total_revenue|
|---|---|
|BluePoloShirt-Mens|217683|
|GreyFashionJacket-Womens|209304|
|WhiteTeeShirt-Mens|152000|

 - **Question 2: What is the total quantity, revenue and discount for each segment?**

```tsql
select segment_id, segment_name,
       sum (qty) as total_qty,
       sum (qty * s.price) as total_reevenue,
       sum (qty * s.price * discount/100) as total_discount
from [balanced_tree.sales] s
join [balanced_tree.product_details] d on s.prod_id = d.product_id
group by segment_id, segment_name
order by segment_id;
```

|segment_id|segment_name|total_qty|total_reevenue|total_discount|
|---|---|---|---|---|
|3|Jeans|11349|208350|23673|
|4|Jacket|11385|366983|42451|
|5|Shirt|11265|406143|48082|
|6|Socks|11217|307977|35280|

 - **Question 3: What is the top selling product for each segment?**

```tsql
with cte_rank as (select segment_id, segment_name, product_id, product_name,
                         sum (qty) as total_qty,
                         dense_rank () over (partition by segment_id order by sum (qty) desc) as rank
                  from [balanced_tree.sales] s
                  join [balanced_tree.product_details] d on s.prod_id = d.product_id
                  group by segment_id, segment_name, product_id, product_name)

select segment_id, segment_name, product_id, product_name, total_qty
from cte_rank
where rank = 1
order by segment_id, product_id;
```

|segment_id|segment_name|product_id|product_name|total_qty|
|---|---|---|---|---|
|3|Jeans|c4a632|NavyOversizedJeans-Womens|3856|
|4|Jacket|9ec847|GreyFashionJacket-Womens|3876|
|5|Shirt|2a2353|BluePoloShirt-Mens|3819|
|6|Socks|f084eb|NavySolidSocks-Mens|3792|

 - **Question 4: What is the total quantity, revenue and discount for each category?**

```tsql
select category_id, category_name,
       sum (qty) as total_qty,
       sum (qty * s.price) as total_reevenue,
       sum (qty * s.price * discount/100) as total_discount
from [balanced_tree.sales] s
join [balanced_tree.product_details] d on s.prod_id = d.product_id
group by category_id, category_name
order by category_id;
```

|category_id|category_name|total_qty|total_reevenue|total_discount|
|---|---|---|---|---|
|1|Womens|22734|575333|66124|
|2|Mens|22482|714120|83362|

 - **Question 5: What is the top selling product for each category?**

```tsql
with cte_rank as (select category_id, category_name, product_id, product_name,
                         sum (qty) as total_qty,
                         dense_rank () over (partition by category_id order by sum (qty) desc) as rank
                  from [balanced_tree.sales] s
                  join [balanced_tree.product_details] d on s.prod_id = d.product_id
                  group by category_id, category_name, product_id, product_name)

select category_id, category_name, product_id, product_name, total_qty
from cte_rank
where rank = 1
order by category_id, product_id;
```

|category_id|category_name|product_id|product_name|total_qty|
|---|---|---|---|---|
|1|Womens|9ec847|GreyFashionJacket-Womens|3876|
|2|Mens|2a2353|BluePoloShirt-Mens|3819|

 - **Question 6: What is the percentage split of revenue by product for each segment?**

```tsql
with cte_revennue as (select segment_id, segment_name, product_id, product_name,
                             sum (qty * s.price) as revenue
                      from [balanced_tree.sales] s
                      join [balanced_tree.product_details] d on s.prod_id = d.product_id
                      group by segment_id, segment_name, product_id, product_name)

select segment_id, segment_name, product_id, product_name,
       cast (round (100.0 * revenue / sum (revenue) over (partition by segment_id),2) as float) as [percentage]
from cte_revennue
order by segment_id;
```

|segment_id|segment_name|product_id|product_name|percentage|
|---|---|---|---|---|
|3|Jeans|c4a632|NavyOversizedJeans-Womens|24.06|
|3|Jeans|e31d39|CreamRelaxedJeans-Womens|17.79|
|3|Jeans|e83aa3|BlackStraightJeans-Womens|58.15|
|4|Jacket|72f5d4|IndigoRainJacket-Womens|19.45|
|4|Jacket|9ec847|GreyFashionJacket-Womens|57.03|
|4|Jacket|d5e9a6|KhakiSuitJacket-Womens|23.51|
|5|Shirt|2a2353|BluePoloShirt-Mens|53.6|
|5|Shirt|5d267b|WhiteTeeShirt-Mens|37.43|
|5|Shirt|c8d436|TealButtonUpShirt-Mens|8.98|
|6|Socks|2feb6b|PinkFluroPolkadotSocks-Mens|35.5|
|6|Socks|b9a74d|WhiteStripedSocks-Mens|20.18|
|6|Socks|f084eb|NavySolidSocks-Mens|44.33|

 - **Question 7: What is the percentage split of revenue by segment for each category?**

```tsql
with cte_revennue as (select category_id, category_name, segment_id, segment_name,
                             sum (qty * s.price) as revenue
                      from [balanced_tree.sales] s
                      join [balanced_tree.product_details] d on s.prod_id = d.product_id
                      group by category_id, category_name, segment_id, segment_name)

select category_id, category_name, segment_id, segment_name,
       cast (round (100.0 * revenue / sum (revenue) over (partition by category_id),2) as float) as [percentage]
from cte_revennue;
```

|category_id|category_name|segment_id|segment_name|percentage|
|---|---|---|---|---|
|1|Womens|3|Jeans|36.21|
|1|Womens|4|Jacket|63.79|
|2|Mens|5|Shirt|56.87|
|2|Mens|6|Socks|43.13|

 - **Question 8: What is the percentage split of total revenue by category?**

```tsql
with cte_revennue as (select category_id, category_name,
                             sum (qty * s.price) as revenue
                      from [balanced_tree.sales] s
                      join [balanced_tree.product_details] d on s.prod_id = d.product_id
                      group by category_id, category_name)

select category_id, category_name,
       cast (round (100.0 * revenue / sum (revenue) over (),2) as float) as [percentage]
from cte_revennue;
```

|category_id|category_name|percentage|
|---|---|---|
|2|Mens|55.38|
|1|Womens|44.62|

 - **Question 9: What is the total transaction “penetration” for each product? (hint: penetration = number of transactions where at least 1 quantity of a product was purchased divided by total number of transactions)**
 - **Question 10: What is the most common combination of at least 1 quantity of any 3 products in a 1 single transaction?**









---
<a name="report"></a>
### **D. Reporting Challenge**


**Write a single SQL script that combines all of the previous questions into a scheduled report that the Balanced Tree team can run at the beginning of each month to calculate the previous month’s values.**

**Imagine that the Chief Financial Officer (which is also Danny) has asked for all of these questions at the end of every month.**

**He first wants you to generate the data for January only - but then he also wants you to demonstrate that you can easily run the samne analysis for February without many changes (if at all).**

**Feel free to split up your final outputs into as many tables as you need - but be sure to explicitly reference which table outputs relate to which question for full marks :)**


---
<a name="bonus"></a>
### **E. Bonus Challenge**

**Use a single SQL query to transform the product_hierarchy and product_prices datasets to the product_details table.**

**Hint: you may want to consider using a recursive CTE to solve this problem!**












