# :tada: CASE STUDY #8 - FRESH SEGMENTS

Find the full case study [**here**](https://8weeksqlchallenge.com/case-study-8/).

## :books: Table of Contents
 - [Introduction](#introduction)
 - [Entity Relationship Diagram](#entity)
 - [Example Datasets](#example)
 - [Questions and Solution](#solution)
   - [A. Data Exploration and Cleansing](#cleansing)
   - [B. Interest Analysis](#interest)
   - [C. Segment Analysis](#segment)
   - [D. Index Analysis](#index)


---
<a name="introduction"></a>
## :question: Introduction
Danny created Fresh Segments, a digital marketing agency that helps other businesses analyse trends in online ad click behaviour for their unique customer base.

Clients share their customer lists with the Fresh Segments team who then aggregate interest metrics and generate a single dataset worth of metrics for further analysis.

In particular - the composition and rankings for different interests are provided for each client showing the proportion of their customer list who interacted with online assets related to each interest for each month.

Danny has asked for your assistance to analyse aggregated metrics for an example client and provide some high level insights about the customer list and their interests.

---

<a name="entity"></a>
## :bookmark: Entity Relationship Diagram

---

<a name="example"></a>
## :open_book: Example Datasets

**Table 1: interest_metrics**

|_month|_year|month_year|interest_id|composition|index_value|ranking|percentile_ranking|
|:----|:----|:----|:----|:----|:----|:----|:----|
|7|2018|07-2018|32486|11.89|6.19|1|99.86|
|7|2018|07-2018|6106|9.93|5.31|2|99.73|
|7|2018|07-2018|18923|10.85|5.29|3|99.59|
|7|2018|07-2018|6344|10.32|5.1|4|99.45|
|7|2018|07-2018|100|10.77|5.04|5|99.31|
|7|2018|07-2018|69|10.82|5.03|6|99.18|
|7|2018|07-2018|79|11.21|4.97|7|99.04|
|7|2018|07-2018|6111|10.71|4.83|8|98.9|
|7|2018|07-2018|6214|9.71|4.83|8|98.9|
|7|2018|07-2018|19422|10.11|4.81|10|98.63|



**Table 2: interest_map**

|id|interest_name|interest_summary|created_at|last_modified|
|:----|:----|:----|:----|:----|
|1|Fitness Enthusiasts|Consumers using fitness tracking apps and websites.|2016-05-26 14:57:59|2018-05-23 11:30:12|
|2|Gamers|Consumers researching game reviews and cheat codes.|2016-05-26 14:57:59|2018-05-23 11:30:12|
|3|Car Enthusiasts|Readers of automotive news and car reviews.|2016-05-26 14:57:59|2018-05-23 11:30:12|
|4|Luxury Retail Researchers|Consumers researching luxury product reviews and gift ideas.|2016-05-26 14:57:59|2018-05-23 11:30:12|
|5|Brides & Wedding Planners|People researching wedding ideas and vendors.|2016-05-26 14:57:59|2018-05-23 11:30:12|
|6|Vacation Planners|Consumers reading reviews of vacation destinations and accommodations.|2016-05-26 14:57:59|2018-05-23 11:30:13|
|7|Motorcycle Enthusiasts|Readers of motorcycle news and reviews.|2016-05-26 14:57:59|2018-05-23 11:30:13|
|8|Business News Readers|Readers of online business news content.|2016-05-26 14:57:59|2018-05-23 11:30:12|
|12|Thrift Store Shoppers|Consumers shopping online for clothing at thrift stores and researching locations.|2016-05-26 14:57:59|2018-03-16 13:14:00|
|13|Advertising Professionals|People who read advertising industry news.|2016-05-26 14:57:59|2018-05-23 11:30:12|


---
<a name="solution"></a>
## :boom: Questions and Solution
<a name="cleansing"></a>
### **A. Data Exploration and Cleansing**
 - **Question 1: Update the fresh_segments.interest_metrics table by modifying the month_year column to be a date data type with the start of the month**

```tsql
alter table [fresh_segments.interest_metrics]
alter column month_year varchar (10);

UPDATE [fresh_segments.interest_metrics]
SET month_year =  CONVERT(DATE, '01-' + month_year, 105);

ALTER TABLE [fresh_segments.interest_metrics]
ALTER COLUMN month_year DATE;
```
|month|year|month_year|interest_id|composition|index_value|ranking|percentile_ranking|
|---|---|---|---|---|---|---|---|
|7|2018|2018-07-01|32486|11.89|6.19|1|99.86|
|7|2018|2018-07-01|6106|9.93|5.31|2|99.73|
|7|2018|2018-07-01|18923|10.85|5.29|3|99.59|
|7|2018|2018-07-01|6344|10.32|5.1|4|99.45|
|7|2018|2018-07-01|100|10.77|5.04|5|99.31|
|7|2018|2018-07-01|69|10.82|5.03|6|99.18|
|7|2018|2018-07-01|79|11.21|4.97|7|99.04|
|7|2018|2018-07-01|6111|10.71|4.83|8|98.9|
|7|2018|2018-07-01|6214|9.71|4.83|8|98.9|
|7|2018|2018-07-01|19422|10.11|4.81|10|98.63|
 - **Question 2: What is count of records in the fresh_segments.interest_metrics for each month_year value sorted in chronological order (earliest to latest) with the null values appearing first?**

```tsql
select month_year, count (*) as [count]
from [fresh_segments.interest_metrics]
group by month_year
order by month_year;
```

|month_year|count|
|---|---|
|NULL|1194|
|2018-07-01|729|
|2018-08-01|767|
|2018-09-01|780|
|2018-10-01|857|
|2018-11-01|928|
|2018-12-01|995|
|2019-01-01|973|
|2019-02-01|1121|
|2019-03-01|1136|
|2019-04-01|1099|
|2019-05-01|857|
|2019-06-01|824|
|2019-07-01|864|
|2019-08-01|1149|

 - **Question 3: What do you think we should do with these null values in the fresh_segments.interest_metrics**

```tsql
select *
from [fresh_segments.interest_metrics]
where month_year is null;
```
| _month | _year | month_year | interest_id | composition | index_value | ranking | percentile_ranking | new_month_year |
| ------ | ----- | ---------- | ----------- | ----------- | ----------- | ------- | ------------------ | -------------- |
| NULL   | NULL  | NULL       | NULL        | 6.12        | 2.85        | 43      | 96.4               | NULL           |
| NULL   | NULL  | NULL       | NULL        | 7.13        | 2.84        | 45      | 96.23              | NULL           |
| NULL   | NULL  | NULL       | NULL        | 6.82        | 2.84        | 45      | 96.23              | NULL           |
| NULL   | NULL  | NULL       | NULL        | 5.96        | 2.83        | 47      | 96.06              | NULL           |
| NULL   | NULL  | NULL       | NULL        | 7.73        | 2.82        | 48      | 95.98              | NULL           |

```tsql
select month_year,
       count (*) as [count],
       round (100.0 * count (*)/ (select count(*) from [dbo].[fresh_segments.interest_metrics]),2) as [percentage]
from [fresh_segments.interest_metrics]
group by month_year
order by month_year;
```




```tsql
delete from [fresh_segments.interest_metrics]
where interest_id is null;
```


 - **Question 4: How many interest_id values exist in the fresh_segments.interest_metrics table but not in the fresh_segments.interest_map table? What about the other way around?**

```tsql
select count (distinct interest_id) as count_interest_id_metrics,
       count (distinct id) as count_id_map,
       sum (case when interest_id is null then 1 else 0 end) as count_not_in_metrics,
       sum (case when id is null then 1 else 0 end) as count_not_in_map
from [fresh_segments.interest_metrics] me
full join [dbo].[fresh_segments.interest_map.] ma on me.interest_id = ma.id;
```

|count_interest_id_metrics|count_id_map|count_not_in_metrics|count_not_in_map|
|---|---|---|---|
|1202|1209|7|0|

 - **Question 5: Summarise the id values in the fresh_segments.interest_map by its total record count in this table**

```tsql
select count (*) as count_id
from [fresh_segments.interest_map.];
```
|count_id|
|---|
|1209|

 - **Question 6: What sort of table join should we perform for our analysis and why? Check your logic by checking the rows where interest_id = 21246 in your joined output and include all columns from fresh_segments.interest_metrics and all columns from fresh_segments.interest_map except from the id column.**


```tsql
select me.*, interest_name, interest_summary, created_at, last_modified
from [fresh_segments.interest_metrics] me
join [fresh_segments.interest_map.] ma on ma.id = me.interest_id
where interest_id = '21246';
```

|month|year|month_year|interest_id|composition|index_value|ranking|percentile_ranking|interest_name|interest_summary|created_at|last_modified|
|---|---|---|---|---|---|---|---|---|---|---|---|
|7|2018|2018-07-01|21246|2.26|0.65|722|0.96|Readers of El Salvadoran Content|People reading news from El Salvadoran media sources.|2018-06-11 17:50:04.000|2018-06-11 17:50:04.000|
|8|2018|2018-08-01|21246|2.13|0.59|765|0.26|Readers of El Salvadoran Content|People reading news from El Salvadoran media sources.|2018-06-11 17:50:04.000|2018-06-11 17:50:04.000|
|9|2018|2018-09-01|21246|2.06|0.61|774|0.77|Readers of El Salvadoran Content|People reading news from El Salvadoran media sources.|2018-06-11 17:50:04.000|2018-06-11 17:50:04.000|
|10|2018|2018-10-01|21246|1.74|0.58|855|0.23|Readers of El Salvadoran Content|People reading news from El Salvadoran media sources.|2018-06-11 17:50:04.000|2018-06-11 17:50:04.000|
|11|2018|2018-11-01|21246|2.25|0.78|908|2.16|Readers of El Salvadoran Content|People reading news from El Salvadoran media sources.|2018-06-11 17:50:04.000|2018-06-11 17:50:04.000|
|12|2018|2018-12-01|21246|1.97|0.7|983|1.21|Readers of El Salvadoran Content|People reading news from El Salvadoran media sources.|2018-06-11 17:50:04.000|2018-06-11 17:50:04.000|
|1|2019|2019-01-01|21246|2.05|0.76|954|1.95|Readers of El Salvadoran Content|People reading news from El Salvadoran media sources.|2018-06-11 17:50:04.000|2018-06-11 17:50:04.000|
|2|2019|2019-02-01|21246|1.84|0.68|1109|1.07|Readers of El Salvadoran Content|People reading news from El Salvadoran media sources.|2018-06-11 17:50:04.000|2018-06-11 17:50:04.000|
|3|2019|2019-03-01|21246|1.75|0.67|1123|1.14|Readers of El Salvadoran Content|People reading news from El Salvadoran media sources.|2018-06-11 17:50:04.000|2018-06-11 17:50:04.000|
|4|2019|2019-04-01|21246|1.58|0.63|1092|0.64|Readers of El Salvadoran Content|People reading news from El Salvadoran media sources.|2018-06-11 17:50:04.000|2018-06-11 17:50:04.000|
|NULL|NULL|NULL|21246|1.61|0.68|1191|0.25|Readers of El Salvadoran Content|People reading news from El Salvadoran media sources.|2018-06-11 17:50:04.000|2018-06-11 17:50:04.000|

 - **Question 7: Are there any records in your joined table where the month_year value is before the created_at value from the fresh_segments.interest_map table? Do you think these values are valid and why?**

```tsql
select count (interest_id) as count_id
from [fresh_segments.interest_metrics] me
join [fresh_segments.interest_map.] ma on ma.id = me.interest_id
where month_year < created_at;
```
|count_id|
|---|
|188|

```tsql
select count (interest_id) as count_id
from [fresh_segments.interest_metrics] me
join [fresh_segments.interest_map.] ma on ma.id = me.interest_id
where datetrunc (month, month_year) < datetrunc (month, created_at);
```

|count_id|
|---|
|0|

***--> moth_year is frist day in month so we use datetrunc that to comparing with month -> result is 0 id***


---

<a name="interest"></a>
### **B. Interest Analysis**

 - **Question 1: Which interests have been present in all month_year dates in our dataset?**

```tsql
select interest_id,
       count (interest_id) as count_id
from [fresh_segments.interest_metrics]
group by interest_id
having count (interest_id) >= (select count (distinct month_year) as count_month
                               from [fresh_segments.interest_metrics])
order by interest_id;
```

|interest_id|count_id|
|---|---|
|100|14|
|10008|14|
|10009|14|
|10010|14|
|101|14|
|102|14|
|10249|14|
|10250|14|
|10251|14|
|10284|14|

***The result has 480 rows***


 - **Question 2: Using this same total_months measure - calculate the cumulative percentage of all records starting at 14 months - which total_months value passes the 90% cumulative percentage value?**

```tsql
with cte_count_month as (select interest_id,
                                count (interest_id) as count_month
                         from [fresh_segments.interest_metrics]
                         group by interest_id),

     cte_count_id as (select count_month as [month],
                             count (interest_id) as count_id
                      from cte_count_month
                      group by count_month)

select [month],
       count_id,
       cast (100.0 * sum (count_id) over (order by [month] desc)
       /sum (count_id) over () as decimal (10,2)) as cumulative_percentage
from cte_count_id
order by [month] desc;
```

|month|count_id|cumulative_percentage|
|---|---|---|
|14|480|39.93|
|13|82|46.76|
|12|65|52.16|
|11|95|60.07|
|10|85|67.14|
|9|95|75.04|
|8|67|80.62|
|7|90|88.10|
|6|33|90.85|
|5|38|94.01|
|4|32|96.67|
|3|15|97.92|
|2|12|98.92|
|1|13|100.00|



 - **Question 3: If we were to remove all interest_id values which are lower than the total_months value we found in the previous question - how many total data points would we be removing?**

```tsql

with cte_interest_remove as (select interest_id,
                                    count (interest_id) as count_month
                             from [fresh_segments.interest_metrics]
                             group by interest_id
                             having count (interest_id) > 6)

select count (interest_id) as interest_id_remove
from [fresh_segments.interest_metrics]
where interest_id in (select interest_id 
                      from cte_interest_remove);
```

|interest_id_remove|
|---|
|12482|

 - **Question 4: Does this decision make sense to remove these data points from a business perspective? Use an example where there are all 14 months present to a removed interest example for your arguments - think about what it means to have less months present from a segment perspective.**



 - **Question 5: After removing these interests - how many unique interests are there for each month?**
---

<a name="segment"></a>
### **C. Segment Analysis**

 - **Question 1: Using our filtered dataset by removing the interests with less than 6 months worth of data, which are the top 10 and bottom 10 interests which have the largest composition values in any month_year? Only use the maximum composition value for each interest but you must keep the corresponding month_year**
 - **Question 2: Which 5 interests had the lowest average ranking value?**
 - **Question 3: Which 5 interests had the largest standard deviation in their percentile_ranking value?**
 - **Question 4: For the 5 interests found in the previous question - what was minimum and maximum percentile_ranking values for each interest and its corresponding year_month value? Can you describe what is happening for these 5 interests?**
 - **Question 5: How would you describe our customers in this segment based off their composition and ranking values? What sort of products or services should we show to these customers and what should we avoid?**
---

<a name="index"></a>
### **D. Index Analysis**
**The index_value is a measure which can be used to reverse calculate the average composition for Fresh Segments’ clients.**

**Average composition can be calculated by dividing the composition column by the index_value column rounded to 2 decimal places.**

 - **Question 1: What is the top 10 interests by the average composition for each month?**
 - **Question 2: For all of these top 10 interests - which interest appears the most often?**
 - **Question 3: What is the average of the average composition for the top 10 interests for each month?**
 - **Question 4: What is the 3 month rolling average of the max average composition value from September 2018 to August 2019 and include the previous top ranking interests in the same output shown below.**
   - **Required output for question 4:**
  

    | "month_year" | "interest_name"               | "max_index_composition" | "3_month_moving_avg" | "1_month_ago"                     | "2_months_ago"                    |
    |--------------|-------------------------------|-------------------------|----------------------|-----------------------------------|-----------------------------------|
    | 2018-09-01   | Work Comes First Travelers    | 8.26                    | 7.61                 | Las Vegas Trip Planners: 7.21     | Las Vegas Trip Planners: 7.21     |
    | 2018-10-01   | Work Comes First Travelers    | 9.14                    | 8.20                 | Work Comes First Travelers: 8.26  | Las Vegas Trip Planners: 8.26     |
    | 2018-11-01   | Work Comes First Travelers    | 8.28                    | 8.56                 | Work Comes First Travelers: 9.14  | Work Comes First Travelers: 9.14  |
    | 2018-12-01   | Work Comes First Travelers    | 8.31                    | 8.58                 | Work Comes First Travelers: 8.28  | Work Comes First Travelers: 8.28  |
    | 2019-01-01   | Work Comes First Travelers    | 7.66                    | 8.08                 | Work Comes First Travelers: 8.31  | Work Comes First Travelers: 8.31  |
    | 2019-02-01   | Work Comes First Travelers    | 7.66                    | 7.88                 | Work Comes First Travelers: 7.66  | Work Comes First Travelers: 7.66  |
    | 2019-03-01   | Alabama Trip Planners         | 6.54                    | 7.29                 | Work Comes First Travelers: 7.66  | Work Comes First Travelers: 7.66  |
    | 2019-04-01   | Solar Energy Researchers      | 6.28                    | 6.83                 | Alabama Trip Planners: 6.54       | Work Comes First Travelers: 6.54  |
    | 2019-05-01   | Readers of Honduran Content   | 4.41                    | 5.74                 | Solar Energy Researchers: 6.28    | Alabama Trip Planners: 6.28       |
    | 2019-06-01   | Las Vegas Trip Planners       | 2.77                    | 4.49                 | Readers of Honduran Content: 4.41 | Solar Energy Researchers: 4.41    |
    | 2019-07-01   | Las Vegas Trip Planners       | 2.82                    | 3.33                 | Las Vegas Trip Planners: 2.77     | Readers of Honduran Content: 2.77 |
    | 2019-08-01   | Cosmetics and Beauty Shoppers | 2.73                    | 2.77                 | Las Vegas Trip Planners: 2.82     | Las Vegas Trip Planners: 2.82     |

   

 - **Question 5: Provide a possible reason why the max average composition might change from month to month? Could it signal something is not quite right with the overall business model for Fresh Segments?**

