# :tada: CASE STUDY #5 - DATA MART

Find the full case study [**here**](https://8weeksqlchallenge.com/case-study-5/).

## :books: Table of Contents
 - [Introduction](#introduction)
 - [Entity Relationship Diagram](#entity)
 - [Example Datasets](#example)
 - [Questions and Solution](#solution)
   - [A. Data Cleansing Steps](#cleansing)
   - [B. Data Exploration](#exploration)
   - [C. Before & After Analysis](#analysis)
   - [D. Bonus Question](#bonus)

---
<a name="introduction"></a>
## :question: Introduction

Subscription based businesses are super popular and Danny realised that there was a large gap in the market - he wanted to create a new streaming service that only had food related content - something like Netflix but with only cooking shows!

Danny finds a few smart friends to launch his new startup Foodie-Fi in 2020 and started selling monthly and annual subscriptions, giving their customers unlimited on-demand access to exclusive food videos from around the world!

Danny created Foodie-Fi with a data driven mindset and wanted to ensure all future investment decisions and new features were decided using data. This case study focuses on using subscription style digital data to answer important business questions.

---

<a name="entity"></a>
## :bookmark: Entity Relationship Diagram

---

<a name="example"></a>
## :open_book: Example Datasets

**Table: Weekly_sales**
| week_date | region        | platform | segment | customer_type | transactions | sales      |
| :-------- | :------------ | :------- | :------ | :------------ | :----------- | :--------- |
| 9/9/20    | OCEANIA       | Shopify  | C3      | New           | 610          | 110033.89  |
| 29/7/20   | AFRICA        | Retail   | C1      | New           | 110692       | 3053771.19 |
| 22/7/20   | EUROPE        | Shopify  | C4      | Existing      | 24           | 8101.54    |
| 13/5/20   | AFRICA        | Shopify  | null    | Guest         | 5287         | 1003301.37 |
| 24/7/19   | ASIA          | Retail   | C1      | New           | 127342       | 3151780.41 |
| 10/7/19   | CANADA        | Shopify  | F3      | New           | 51           | 8844.93    |
| 26/6/19   | OCEANIA       | Retail   | C3      | New           | 152921       | 5551385.36 |
| 29/5/19   | SOUTH AMERICA | Shopify  | null    | New           | 53           | 10056.2    |
| 22/8/18   | AFRICA        | Retail   | null    | Existing      | 31721        | 1718863.58 |
| 25/7/18   | SOUTH AMERICA | Retail   | null    | New           | 2136         | 81757.91   |

---

<a name="solution"></a>
## :boom: Questions and Solution
<a name="cleansing"></a>
### **A. Data Cleansing Steps**

**In a single query, perform the following operations and generate a new table in the data_mart schema named clean_weekly_sales:**
 - **Convert the week_date to a DATE format**
 - **Add a week_number as the second column for each week_date value, for example any value from the 1st of January to 7th of January will be 1, 8th to 14th will be 2 etc**
 - **Add a month_number with the calendar month for each week_date value as the 3rd column**
 - **Add a calendar_year column as the 4th column containing either 2018, 2019 or 2020 values**
 - **Add a new column called age_band after the original segment column using the following mapping on the number inside the segment value**

|segment|	age_band|
|---|---|
|1|	Young Adults|
|2| Middle Aged|
|3 or 4| Retirees|

 - **Add a new demographic column using the following mapping for the first letter in the segment values:**

|segment|	demographic|
|---|---|
|C|	Couples|
|F| Families|

 - **Ensure all null string values with an "unknown" string value in the original segment column as well as the new age_band and demographic columns**
 - **Generate a new avg_transaction column as the sales value divided by transactions rounded to 2 decimal places for each record**

***Query***


```tsql
drop table if exists ##clean_weekly_sales
select week_date,
       datepart (week,week_date) as week_number,
       month (week_date) as month_number,
       year (week_date) as calendar_year,
       region, platform, segment,
       case when right (segment, 1) = '1' then 'Young Adults'
            when right (segment,1) = '2' then 'Middle Aged' 
            when right (segment,1) in ('3','4') then 'Retirees'
            else 'Unknow'
       end as age_band,
       case when left (segment, 1) = 'C' then 'Couples'
            when left (segment, 1) = 'F' then 'Families'
            else 'Unknow'
       end as demographic,
       customer_type, transactions,
       round (sales/transactions,2) as avg_transaction,
       sales
into ##clean_weekly_sales
from data_mart;
```
***New table: ##clean_weekly_sales***
|week_date|week_number|month_number|calendar_year|region|platform|segment|age_band|demographic|customer_type|transactions|avg_transaction|sales|
|---|---|---|---|---|---|---|---|---|---|---|---|---|
|2020-08-31|36|8|2020|ASIA|Retail|C3|Retirees|Couples|New|120631|30|3656163|
|2020-08-31|36|8|2020|ASIA|Retail|F1|Young Adults|Families|New|31574|31|996575|
|2020-08-31|36|8|2020|USA|Retail|null|Unknow|Unknow|Guest|529151|31|16509610|
|2020-08-31|36|8|2020|EUROPE|Retail|C1|Young Adults|Couples|New|4517|31|141942|
|2020-08-31|36|8|2020|AFRICA|Retail|C2|Middle Aged|Couples|New|58046|30|1758388|
|2020-08-31|36|8|2020|CANADA|Shopify|F2|Middle Aged|Families|Existing|1336|182|243878|
|2020-08-31|36|8|2020|AFRICA|Shopify|F3|Retirees|Families|Existing|2514|206|519502|
|2020-08-31|36|8|2020|ASIA|Shopify|F1|Young Adults|Families|Existing|2158|172|371417|
|2020-08-31|36|8|2020|AFRICA|Shopify|F2|Middle Aged|Families|New|318|155|49557|
|2020-08-31|36|8|2020|AFRICA|Retail|C3|Retirees|Couples|New|111032|35|3888162|

---


<a name="exploration"></a>
### **B. Data Exploration**
 - **Question 1: What day of the week is used for each week_date value?**

```tsql
select distinct datename (weekday,week_date) as week_day
from ##clean_weekly_sales;
```

|week_day|
|---|
|Monday|

 - **Question 2: What range of week numbers are missing from the dataset?**

```tsql
with cte_week_number as (select value as week_number_of_year 
                         from generate_series (1,52))

select w.week_number_of_year
from cte_week_number w
left join ##clean_weekly_sales c on c.week_number = w.week_number_of_year
where c.week_number is null
order by w.week_number_of_year;
```

|week_number_of_year|
|---|
|1|
|2|
|3|
|4|
|5|
|6|
|7|
|8|
|9|
|10|

***The result has 28 rows***

 - **Question 3: How many total transactions were there for each year in the dataset?**

```tsql
select calendar_year,
       sum (transactions) as total_transactions
from ##clean_weekly_sales
group by calendar_year
order by calendar_year;
```

|calendar_year|total_transactions|
|---|---|
|2018|346406460|
|2019|365639285|
|2020|375813651|

 - **Question 4: What is the total sales for each region for each month?**

```tsql
select month_number, region,
       sum (cast (sales as float)) as total_sales
from ##clean_weekly_sales
group by month_number, region
order by month_number, region;
```

|month_number|region|total_sales|
|---|---|---|
|3|AFRICA|567767480|
|3|ASIA|529770793|
|3|CANADA|144634329|
|3|EUROPE|35337093|
|3|OCEANIA|783282888|
|3|SOUTHAMERICA|71023109|
|3|USA|225353043|
|4|AFRICA|1911783504|
|4|ASIA|1804628707|
|4|CANADA|484552594|

***The result has 49 rows***


 - **Question 5: What is the total count of transactions for each platform**

```tsql
select platform,
       count (transactions) as count_transaction 
from ##clean_weekly_sales
group by platform;
```

|platform|count_transaction|
|---|---|
|Retail|8568|
|Shopify|8549|

 - **Question 6: What is the percentage of sales for Retail vs Shopify for each month?**

```tsql
select calendar_year, month_number,
       round (100 * sum (case when platform = 'Retail' then cast (sales as float) end) / sum (cast (sales as float)),2) as percentage_sales_retail,
       round (100 * sum (case when platform = 'Shopify' then cast (sales as float) end) / sum (cast (sales as float)),2) as percentage_sales_shopify
from ##clean_weekly_sales
group by calendar_year, month_number
order by calendar_year, month_number;
```

|calendar_year|month_number|percentage_sales_retail|percentage_sales_shopify|
|---|---|---|---|
|2018|3|97.92|2.08|
|2018|4|97.93|2.07|
|2018|5|97.73|2.27|
|2018|6|97.76|2.24|
|2018|7|97.75|2.25|
|2018|8|97.71|2.29|
|2018|9|97.68|2.32|
|2019|3|97.71|2.29|
|2019|4|97.8|2.2|
|2019|5|97.52|2.48|

***The result has 20 rows***

 - **Question 7: What is the percentage of sales by demographic for each year in the dataset?**

```tsql
select calendar_year,
       round (100 * sum (case when demographic = 'Couples' then cast (sales as float) end) / sum (cast (sales as float)),2) as percentage_sales_couples,
       round (100 * sum (case when demographic = 'Families' then cast (sales as float) end) / sum (cast (sales as float)),2) as percentage_sales_families,
       round (100 * sum (case when demographic = 'Unknow' then cast (sales as float) end) / sum (cast (sales as float)),2) as percentage_sales_unknow
from ##clean_weekly_sales
group by calendar_year
order by calendar_year;
```

|calendar_year|percentage_sales_couples|percentage_sales_families|percentage_sales_unknow|
|---|---|---|---|
|2018|26.38|31.99|41.63|
|2019|27.28|32.47|40.25|
|2020|28.72|32.73|38.55|

 - **Question 8: Which age_band and demographic values contribute the most to Retail sales?**

```tsql
select age_band, demographic,
       sum (cast (sales as float)) as total_sales,
       round (100 * sum (cast (sales as float))/(select sum (cast (sales as float))
                                                 from ##clean_weekly_sales
                                                 where platform = 'Retail'),2) as percentage
from ##clean_weekly_sales
where platform = 'Retail'
group by age_band, demographic
order by total_sales desc, age_band, demographic;
```

|age_band|demographic|total_sales|percentage|
|---|---|---|---|
|Unknow|Unknow|16067285533|40.52|
|Retirees|Families|6634686916|16.73|
|Retirees|Couples|6370580014|16.07|
|Middle Aged|Families|4354091554|10.98|
|Young Adults|Couples|2602922797|6.56|
|Middle Aged|Couples|1854160330|4.68|
|Young Adults|Families|1770889293|4.47|

 - **Question 9: Can we use the avg_transaction column to find the average transaction size for each year for Retail vs Shopify? If not - how would you calculate it instead?**

```tsql
select calendar_year, platform,
       avg (avg_transaction) as total_avg_trans_row,
       round (sum (cast (sales as float)) / sum (transactions) , 2) as avg_transactions
from ##clean_weekly_sales
group by calendar_year, platform
order by calendar_year, platform;
```

|calendar_year|platform|total_avg_trans_row|avg_transactions|
|---|---|---|---|
|2018|Retail|42|36.56|
|2018|Shopify|187|192.48|
|2019|Retail|41|36.83|
|2019|Shopify|177|183.36|
|2020|Retail|40|36.56|
|2020|Shopify|174|179.03|



---

<a name="analysis"></a>
### **C. Before & After Analysis**


**This technique is usually used when we inspect an important event and want to inspect the impact before and after a certain point in time.**

**Taking the week_date value of 2020-06-15 as the baseline week where the Data Mart sustainable packaging changes came into effect.**

**We would include all week_date values for 2020-06-15 as the start of the period after the change and the previous week_date values would be before**

**Using this analysis approach - answer the following questions:**




```tsql
select distinct (week_number)
from ##clean_weekly_sales
where week_date = '2020-06-15';
```
|week_number|
|---|
|25|

***Before analyzing, we find the week_date values for 2020-06-15 is 25***



 - **Question 1: What is the total sales for the 4 weeks before and after 2020-06-15? What is the growth or reduction rate in actual values and percentage of sales?**

```tsql
with cte_week_sales as (select week_date, week_number, sales
                        from ##clean_weekly_sales
                        where calendar_year = 2020 and week_number between (25-4) and (25+3)),

     cte_summations as (select sum (case when week_number between (25-4) and (25-1) then cast (sales as float) end) as total_sales_before,
                               sum (case when week_number between 25 and (25+3) then cast (sales as float) end) as total_sales_after 
                        from cte_week_sales)

select total_sales_before, total_sales_after,
       total_sales_after - total_sales_before as diff,
       round (100 * (total_sales_after - total_sales_before)/total_sales_before,2) as percentage
from cte_summations;
```

|total_sales_before|total_sales_after|diff|percentage|
|---|---|---|---|
|2345878357|2318994169|-26884188|-1.15|

 - **Question 2: What about the entire 12 weeks before and after?**

```tsql
with cte_week_sales as (select week_date, week_number, sales
                        from ##clean_weekly_sales
                        where calendar_year = 2020 and week_number between (25-12) and (25+11)),

     cte_summations as (select sum (case when week_number between (25-12) and (25-1) then cast (sales as float) end) as total_sales_before,
                               sum (case when week_number between 25 and (25+11) then cast (sales as float) end) as total_sales_after 
                        from cte_week_sales)

select total_sales_before, total_sales_after,
       total_sales_after - total_sales_before as diff,
       round (100 * (total_sales_after - total_sales_before)/total_sales_before,2) as percentage
from cte_summations;

```

|total_sales_before|total_sales_after|diff|percentage|
|---|---|---|---|
|7126273147|6973947753|-152325394|-2.14|

 - **Question 3: How do the sale metrics for these 2 periods before and after compare with the previous years in 2018 and 2019?**


***How do the sale metrics for 4 weeks before and after compare with the previous years in 2018 and 2019?***
```tsql
with cte_week_sales as (select week_date, week_number, sales,calendar_year
                        from ##clean_weekly_sales
                        where calendar_year != 2020 and week_number between (25-4) and (25+3)),

     cte_summations as (select calendar_year,
                               sum (case when week_number between (25-4) and (25-1) then cast (sales as float) end) as total_sales_before,
                               sum (case when week_number between 25 and (25+3) then cast (sales as float) end) as total_sales_after 
                        from cte_week_sales
                        group by calendar_year)

select calendar_year,
       total_sales_before, total_sales_after,
       total_sales_after - total_sales_before as diff,
       round (100 * (total_sales_after - total_sales_before)/total_sales_before,2) as percentage
from cte_summations
order by calendar_year;
```

|calendar_year|total_sales_before|total_sales_after|diff|percentage|
|---|---|---|---|---|
|2018|2125140809|2129242914|4102105|0.19|
|2019|2249989796|2252326390|2336594|0.1|

***How do the sale metrics for 12 weeks before and after compare with the previous years in 2018 and 2019?***


```tsql
with cte_week_sales as (select week_date, week_number, sales, calendar_year
                        from ##clean_weekly_sales
                        where calendar_year != 2020 and week_number between (25-12) and (25+11)),

     cte_summations as (select calendar_year,
                               sum (case when week_number between (25-12) and (25-1) then cast (sales as float) end) as total_sales_before,
                               sum (case when week_number between 25 and (25+11) then cast (sales as float) end) as total_sales_after 
                        from cte_week_sales
                        group by calendar_year)

select calendar_year,
       total_sales_before, total_sales_after,
       total_sales_after - total_sales_before as diff,
       round (100 * (total_sales_after - total_sales_before)/total_sales_before,2) as percentage
from cte_summations
order by calendar_year;
```


|calendar_year|total_sales_before|total_sales_after|diff|percentage|
|---|---|---|---|---|
|2018|6396562317|6500818510|104256193|1.63|
|2019|6883386397|6862646103|-20740294|-0.3|



---

<a name="bonus"></a>
### **D. Bonus Question**

**Which areas of the business have the highest negative impact in sales metrics performance in 2020 for the 12 week before and after period?**
 - **region**
 - **platform**
 - **age_band**
 - **demographic**
 - **customer_type**

**Do you have any further recommendations for Danny’s team at Data Mart or any interesting insights based off this analysis?**





