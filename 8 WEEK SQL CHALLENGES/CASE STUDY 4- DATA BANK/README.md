# :tada: CASE STUDY #4 - DATA BANK

Find the full case study [**here**](https://8weeksqlchallenge.com/case-study-4/).

## :books: Table of Contents
 - [Introduction](#introduction)
 - [Entity Relationship Diagram](#entity)
 - [Example Datasets](#example)
 - [Questions and Solution](#solution)
   - [A. Customer Nodes Exploration](#nodes)
   - [B. Customer Transactions](#transactions)
   - [C. Data Allocation Challenge](#allocation)
   - [D. Extra Challenge](#extra)
   - [E. Extension Request](extension)
---
<a name="introduction"></a>
## :question: Introduction
There is a new innovation in the financial industry called Neo-Banks: new aged digital only banks without physical branches.

Danny thought that there should be some sort of intersection between these new age banks, cryptocurrency and the data world…so he decides to launch a new initiative - Data Bank!

Data Bank runs just like any other digital bank - but it isn’t only for banking activities, they also have the world’s most secure distributed data storage platform!

Customers are allocated cloud data storage limits which are directly linked to how much money they have in their accounts. There are a few interesting caveats that go with this business model, and this is where the Data Bank team need your help!

The management team at Data Bank want to increase their total customer base - but also need some help tracking just how much data storage their customers will need.

This case study is all about calculating metrics, growth and helping the business analyse their data in a smart way to better forecast and plan for their future developments!

---

<a name="entity"></a>
## :bookmark: Entity Relationship Diagram

<img src="https://github.com/NguyenDo76/MENTORSHIP/blob/main/8%20WEEK%20SQL%20CHALLENGES/IMAGE/CASE%20STUDY%204-%20DATA%20BANK.png">

---
<a name="example"></a>
## :open_book: Example Datasets

**Table 1: regions**

| region_id | region_name |
| :-------- | :---------- |
| 1         | Africa      |
| 2         | America     |
| 3         | Asia        |
| 4         | Europe      |
| 5         | Oceania     |

**Table 2: customer_nodes**

| customer_id | region_id | node_id | start_date | end_date   |
| :---------- | :-------- | :------ | :--------- | :--------- |
| 1           | 3         | 4       | 2020-01-02 | 2020-01-03 |
| 2           | 3         | 5       | 2020-01-03 | 2020-01-17 |
| 3           | 5         | 4       | 2020-01-27 | 2020-02-18 |
| 4           | 5         | 4       | 2020-01-07 | 2020-01-19 |
| 5           | 3         | 3       | 2020-01-15 | 2020-01-23 |
| 6           | 1         | 1       | 2020-01-11 | 2020-02-06 |
| 7           | 2         | 5       | 2020-01-20 | 2020-02-04 |
| 8           | 1         | 2       | 2020-01-15 | 2020-01-28 |
| 9           | 4         | 5       | 2020-01-21 | 2020-01-25 |
| 10          | 3         | 4       | 2020-01-13 | 2020-01-14 |

**Table 3: customer_transactions**

| customer_id | txn_date   | txn_type | txn_amount |
| :---------- | :--------- | :------- | :--------- |
| 429         | 2020-01-21 | deposit  | 82         |
| 155         | 2020-01-10 | deposit  | 712        |
| 398         | 2020-01-01 | deposit  | 196        |
| 255         | 2020-01-14 | deposit  | 563        |
| 185         | 2020-01-29 | deposit  | 626        |
| 309         | 2020-01-13 | deposit  | 995        |
| 312         | 2020-01-20 | deposit  | 485        |
| 376         | 2020-01-03 | deposit  | 706        |
| 188         | 2020-01-13 | deposit  | 601        |
| 138         | 2020-01-11 | deposit  | 520        |

---

<a name="solution"></a>
## :boom: Questions and Solution
<a name="nodes"></a>
### **A. Customer Nodes Exploration**
 - **Question 1: How many unique nodes are there on the Data Bank system?**

```tsql
select count (distinct node_id) as count_unique_nodes
from [data_bank.customer_nodes ];
```

|count_unique_nodes|
|---|
|5|

 - **Question 2: What is the number of nodes per region?**

```tsql
select c.region_id, region_name,
       count (distinct node_id) as count_nodes
from [data_bank.customer_nodes ] c
join [data_bank.regions] r on r.region_id = c.region_id
group by c.region_id, region_name
order by c.region_id;
```

|region_id|region_name|count_nodes|
|---|---|---|
|1|Australia|5|
|2|America|5|
|3|Africa|5|
|4|Asia|5|
|5|Europe|5|

 - **Question 3: How many customers are allocated to each region?**

```tsql
select c.region_id, region_name,
       count (customer_id) as count_customer
from [data_bank.customer_nodes ] c
join [data_bank.regions] r on r.region_id = c.region_id
group by c.region_id, region_name
order by c.region_id;
```

|region_id|region_name|count_customer|
|---|---|---|
|1|Australia|770|
|2|America|735|
|3|Africa|714|
|4|Asia|665|
|5|Europe|616|

 - **Question 4: How many days on average are customers reallocated to a different node?**

```tsql
with cte_diff_day as (select customer_id, node_id, 
                             sum (datediff (d,start_date,end_date)) as diff_day
                      from [data_bank.customer_nodes ] c
                      where end_date != '9999-12-31'
                      group by customer_id, node_id)

select round (avg (diff_day),0) as avg_customer
from cte_diff_day;
```

|avg_customer|
|---|
|23|

 - **Question 5: What is the median, 80th and 95th percentile for this same reallocation days metric for each region?**

```tsql
with cte_diff_day as (select customer_id, node_id,region_id,
                             sum (datediff (d,start_date,end_date)) as diff_day
                      from [data_bank.customer_nodes ] c
                      where end_date != '9999-12-31'
                      group by customer_id, node_id,region_id)

select distinct region_name,
                round (percentile_cont (0.5) within group (order by diff_day) over (partition by region_name),2) as median,
                round (percentile_cont (0.8) within group (order by diff_day) over (partition by region_name),2) as P80,
                round (percentile_cont (0.95) within group (order by diff_day) over (partition by region_name),2) as P95
from cte_diff_day d
join [data_bank.regions] r on d.region_id = r.region_id;
```

|region_name|median|P80|P95|
|---|---|---|---|
|Africa|22|35|54|
|America|22|34|53.7|
|Asia|22|34.6|52|
|Europe|23|34|51.4|
|Australia|21|34|51|


---
<a name="transactions"></a>
### **B. Customer Transactions**
 - **Question 1: What is the unique count and total amount for each transaction type?**

```tsql
select txn_type,
       count (distinct customer_id) as count_cus,
       sum (txn_amount) as total_amount
from [data_bank.customer_transactions]
group by txn_type;
```

|txn_type|count_cus|total_amount|
|---|---|---|
|withdrawal|439|793003|
|deposit|500|1359168|
|purchase|448|806537|

 - **Question 2: What is the average total historical deposit counts and amounts for all customers?**

```tsql
with cte_deposit as (select customer_id,
                            count (customer_id) as count_cus,
                            sum (txn_amount) as total_amount
                     from [data_bank.customer_transactions]
                     where txn_type = 'deposit'
                     group by customer_id)

select round (avg (count_cus),0) as avg_cus,
	round (avg (total_amount),0) avg_amount
from cte_deposit;
```

|avg_cus|avg_amount|
|---|---|
|5|2718|

 - **Question 3: For each month - how many Data Bank customers make more than 1 deposit and either 1 purchase or 1 withdrawal in a single month?**

```tsql
with cte_total_type as (select customer_id,
                               datepart (month,txn_date) as [month],
                               sum (case when txn_type = 'deposit' then 1 else 0 end) as total_deposit,
                               sum (case when txn_type = 'purchase' then 1 else 0 end) as total_purchase,
                               sum (case when txn_type = 'withdrawal' then 1 else 0 end) as total_withdrawal
                        from [data_bank.customer_transactions]
                        group by customer_id, datepart (month,txn_date))

select [month],
        count (customer_id) as count_cus
from cte_total_type
where total_deposit > 1 and (total_purchase >= 1 or total_withdrawal >= 1)
group by [month]
order by [month];
```

|month|count_cus|
|---|---|
|1|168|
|2|181|
|3|192|
|4|70|

 - **Question 4: What is the closing balance for each customer at the end of the month?**

```tsql
with cte_total_type as (select customer_id,
                               datepart (month,txn_date) as [month],
                               sum (case when txn_type = 'deposit' then txn_amount else 0 end) as total_deposit,
                               sum (case when txn_type = 'purchase' then txn_amount else 0 end) as total_purchase,
                               sum (case when txn_type = 'withdrawal' then txn_amount else 0 end) as total_withdrawal
                       from [data_bank.customer_transactions]
                       group by customer_id, datepart (month,txn_date)),

     cte_balance_in_month as (select customer_id, month,
                                     total_deposit - total_purchase - total_withdrawal as balance_in_month
                              from cte_total_type),

     cte_closing_balance as (select customer_id, month,
                                    sum (balance_in_month) over (partition by customer_id order by month) AS closing_balance
                             from cte_balance_in_month)

select customer_id, month,
       coalesce (closing_balance, 0) as closing_balance,
       coalesce (closing_balance - lag (closing_balance,1) over (partition by customer_id order by month),0) as change_in_balance
from cte_closing_balance
order by customer_id, month;
```

|customer_id|month|closing_balance|change_in_balance|
|---|---|---|---|
|1|1|312|0|
|1|3|-640|-952|
|2|1|549|0|
|2|3|610|61|
|3|1|144|0|
|3|2|-821|-965|
|3|3|-1222|-401|
|3|4|-729|493|

***The result has 1720 rows***

 - **Question 5: What is the percentage of customers who increase their closing balance by more than 5%?**

```tsql
drop table if exists ##new_transactions;
with cte_total_type as (select customer_id,
                               datepart (month,txn_date) as [month],
                               sum (case when txn_type = 'deposit' then txn_amount else 0 end) as total_deposit,
                               sum (case when txn_type = 'purchase' then txn_amount else 0 end) as total_purchase,
                               sum (case when txn_type = 'withdrawal' then txn_amount else 0 end) as total_withdrawal
                       from [data_bank.customer_transactions]
                       group by customer_id, datepart (month,txn_date)),

     cte_balance_in_month as (select customer_id, month,
                                     total_deposit - total_purchase - total_withdrawal as balance_in_month
                              from cte_total_type),

     cte_closing_balance as (select customer_id, month,
                                    sum (balance_in_month) over (partition by customer_id order by month) AS closing_balance
                             from cte_balance_in_month)

select customer_id, month,
       coalesce (closing_balance, 0) as closing_balance,
       coalesce (closing_balance - lag (closing_balance,1) over (partition by customer_id order by month),0) as change_in_balance
into ##new_transactions
from cte_closing_balance
order by customer_id, month;

```


|customer_id|month|closing_balance|change_in_balance|
|---|---|---|---|
|1|1|312|0|
|1|3|-640|-952|
|2|1|549|0|
|2|3|610|61|
|3|1|144|0|
|3|2|-821|-965|
|3|3|-1222|-401|
|3|4|-729|493|
|4|1|848|0|
|4|3|655|-193|

***The result has 1720 rows***

---
<a name="allocation"></a>
### **C. Data Allocation Challenge**
**To test out a few different hypotheses - the Data Bank team wants to run an experiment where different groups of customers would be allocated data using 3 different options:**
  - **Option 1: data is allocated based off the amount of money at the end of the previous month**
  - **Option 2: data is allocated on the average amount of money kept in the account in the previous 30 days**
  - **Option 3: data is updated real-time**


**For this multi-part challenge question - you have been requested to generate the following data elements to help the Data Bank team estimate how much data will need to be provisioned for each option:**
  - **running customer balance column that includes the impact each transaction**
  - **customer balance at the end of each month**
  - **minimum, average and maximum values of the running balance for each customer**
**Using all of the data available - how much data would have been required for each option on a monthly basis?**
---
<a name="extra"></a>
### **D. Extra Challenge**
**Data Bank wants to try another option which is a bit more difficult to implement - they want to calculate data growth using an interest calculation, just like in a traditional savings account you might have with a bank.**

**If the annual interest rate is set at 6% and the Data Bank team wants to reward its customers by increasing their data allocation based off the interest calculated on a daily basis at the end of each day, how much data would be required for this option on a monthly basis?**

**Special notes:**

- **Data Bank wants an initial calculation which does not allow for compounding interest, however they may also be interested in a daily compounding interest calculation so you can try to perform this calculation if you have the stamina!**
---
<a name="extension"></a>
### **E. Extension Request**

**The Data Bank team wants you to use the outputs generated from the above sections to create a quick Powerpoint presentation which will be used as marketing materials for both external investors who might want to buy Data Bank shares and new prospective customers who might want to bank with Data Bank.**

 1. **Using the outputs generated from the customer node questions, generate a few headline insights which Data Bank might use to market it’s world-leading security features to potential investors and customers.**
 2. **With the transaction analysis - prepare a 1 page presentation slide which contains all the relevant information about the various options for the data provisioning so the Data Bank management team can make an informed decision.**















