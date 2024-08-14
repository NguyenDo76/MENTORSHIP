<<<<<<< HEAD
-- A.DATA EXPLORATION AND CLEANSING
-- 1.Update the fresh_segments.interest_metrics table by modifying the month_year column to be a date data type with the start of the month
alter table [dbo].[fresh_segments.interest_metrics]
alter column month_year varchar (10);

UPDATE [dbo].[fresh_segments.interest_metrics]
SET month_year =  CONVERT(DATE, '01-' + month_year, 105);

ALTER TABLE [dbo].[fresh_segments.interest_metrics]
ALTER COLUMN month_year DATE;

-- 2.What is count of records in the fresh_segments.interest_metrics for each month_year value sorted in chronological order (earliest to latest) with the null values appearing first?
select month_year, count (*) as [count]
from [dbo].[fresh_segments.interest_metrics]
group by month_year
order by month_year;

-- 3.What do you think we should do with these null values in the fresh_segments.interest_metrics
select *
from [dbo].[fresh_segments.interest_metrics]
where month_year is null;

select month_year,
       count (*) as [count],
       round (100.0 * count (*)/ (select count(*) from [dbo].[fresh_segments.interest_metrics]),2) as [percentage]
from [dbo].[fresh_segments.interest_metrics]
group by month_year
order by month_year;

delete from [dbo].[fresh_segments.interest_metrics]
where interest_id is null;

-- 4.How many interest_id values exist in the fresh_segments.interest_metrics table but not in the fresh_segments.interest_map table? What about the other way around?
select count (distinct interest_id) as count_interest_id_metrics,
       count (distinct id) as count_id_map,
       sum (case when interest_id is null then 1 else 0 end) as count_not_in_metrics,
       sum (case when id is null then 1 else 0 end) as count_not_in_map
from [dbo].[fresh_segments.interest_metrics] me
full join [dbo].[fresh_segments.interest_map.] ma on me.interest_id = ma.id;

-- 5.Summarise the id values in the fresh_segments.interest_map by its total record count in this table
select count (*) as count_id
from [dbo].[fresh_segments.interest_map.];

-- 6.What sort of table join should we perform for our analysis and why? Check your logic by checking the rows where interest_id = 21246 in your joined output and include all columns from fresh_segments.interest_metrics and all columns from fresh_segments.interest_map except from the id column.
select me.*, interest_name, interest_summary, created_at, last_modified
from [dbo].[fresh_segments.interest_metrics] me
join [dbo].[fresh_segments.interest_map.] ma on ma.id = me.interest_id
where interest_id = '21246';

-- 7.Are there any records in your joined table where the month_year value is before the created_at value from the fresh_segments.interest_map table? Do you think these values are valid and why?
select count (interest_id) as count_id
from [dbo].[fresh_segments.interest_metrics] me
join [dbo].[fresh_segments.interest_map.] ma on ma.id = me.interest_id
where month_year < created_at;
--> 188 id

select count (interest_id) as count_id
from [dbo].[fresh_segments.interest_metrics] me
join [dbo].[fresh_segments.interest_map.] ma on ma.id = me.interest_id
where datetrunc (month, month_year) < datetrunc (month, created_at);
--> moth_year is frist day in month so we use datetrunc that to comparing with month -> result is 0 id


-- B.INTEREST ANALYSIS
-- 1.Which interests have been present in all month_year dates in our dataset?
select interest_id,
       count (interest_id) as count_id
from [fresh_segments.interest_metrics]
group by interest_id
having count (interest_id) >= (select count (distinct month_year) as count_month
                               from [dbo].[fresh_segments.interest_metrics])
order by interest_id;

-- 2.Using this same total_months measure - calculate the cumulative percentage of all records starting at 14 months - which total_months value passes the 90% cumulative percentage value?
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

-- 3.If we were to remove all interest_id values which are lower than the total_months value we found in the previous question - how many total data points would we be removing?
--we remove month > 6

with cte_interest_remove as (select interest_id,
                                    count (interest_id) as count_month
                             from [fresh_segments.interest_metrics]
                             group by interest_id
                             having count (interest_id) > 6)

select count (interest_id) as interest_id_remove
from [fresh_segments.interest_metrics]
where interest_id in (select interest_id 
                      from cte_interest_remove);

-- 4.Does this decision make sense to remove these data points from a business perspective? Use an example where there are all 14 months present to a removed interest example for your arguments - think about what it means to have less months present from a segment perspective.


-- 5.After removing these interests - how many unique interests are there for each month?
=======
-- A.DATA EXPLORATION AND CLEANSING
-- 1.Update the fresh_segments.interest_metrics table by modifying the month_year column to be a date data type with the start of the month
alter table [dbo].[fresh_segments.interest_metrics]
alter column month_year varchar (10);

UPDATE [dbo].[fresh_segments.interest_metrics]
SET month_year =  CONVERT(DATE, '01-' + month_year, 105);

ALTER TABLE [dbo].[fresh_segments.interest_metrics]
ALTER COLUMN month_year DATE;

-- 2.What is count of records in the fresh_segments.interest_metrics for each month_year value sorted in chronological order (earliest to latest) with the null values appearing first?
select month_year, count (*) as [count]
from [dbo].[fresh_segments.interest_metrics]
group by month_year
order by month_year;

-- 3.What do you think we should do with these null values in the fresh_segments.interest_metrics
select *
from [dbo].[fresh_segments.interest_metrics]
where month_year is null;

select month_year,
       count (*) as [count],
       round (100.0 * count (*)/ (select count(*) from [dbo].[fresh_segments.interest_metrics]),2) as [percentage]
from [dbo].[fresh_segments.interest_metrics]
group by month_year
order by month_year;

delete from [dbo].[fresh_segments.interest_metrics]
where interest_id is null;

-- 4.How many interest_id values exist in the fresh_segments.interest_metrics table but not in the fresh_segments.interest_map table? What about the other way around?
select count (distinct interest_id) as count_interest_id_metrics,
       count (distinct id) as count_id_map,
       sum (case when interest_id is null then 1 else 0 end) as count_not_in_metrics,
       sum (case when id is null then 1 else 0 end) as count_not_in_map
from [dbo].[fresh_segments.interest_metrics] me
full join [dbo].[fresh_segments.interest_map.] ma on me.interest_id = ma.id;

-- 5.Summarise the id values in the fresh_segments.interest_map by its total record count in this table
select count (*) as count_id
from [dbo].[fresh_segments.interest_map.];

-- 6.What sort of table join should we perform for our analysis and why? Check your logic by checking the rows where interest_id = 21246 in your joined output and include all columns from fresh_segments.interest_metrics and all columns from fresh_segments.interest_map except from the id column.
select me.*, interest_name, interest_summary, created_at, last_modified
from [dbo].[fresh_segments.interest_metrics] me
join [dbo].[fresh_segments.interest_map.] ma on ma.id = me.interest_id
where interest_id = '21246';

-- 7.Are there any records in your joined table where the month_year value is before the created_at value from the fresh_segments.interest_map table? Do you think these values are valid and why?
select count (interest_id) as count_id
from [dbo].[fresh_segments.interest_metrics] me
join [dbo].[fresh_segments.interest_map.] ma on ma.id = me.interest_id
where month_year < created_at;
--> 188 id

select count (interest_id) as count_id
from [dbo].[fresh_segments.interest_metrics] me
join [dbo].[fresh_segments.interest_map.] ma on ma.id = me.interest_id
where datetrunc (month, month_year) < datetrunc (month, created_at);
--> moth_year is frist day in month so we use datetrunc that to comparing with month -> result is 0 id


-- B.INTEREST ANALYSIS
-- 1.Which interests have been present in all month_year dates in our dataset?
select interest_id,
       count (interest_id) as count_id
from [fresh_segments.interest_metrics]
group by interest_id
having count (interest_id) >= (select count (distinct month_year) as count_month
                               from [dbo].[fresh_segments.interest_metrics])
order by interest_id;

-- 2.Using this same total_months measure - calculate the cumulative percentage of all records starting at 14 months - which total_months value passes the 90% cumulative percentage value?
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

-- 3.If we were to remove all interest_id values which are lower than the total_months value we found in the previous question - how many total data points would we be removing?
--we remove month > 6

with cte_interest_remove as (select interest_id,
                                    count (interest_id) as count_month
                             from [fresh_segments.interest_metrics]
                             group by interest_id
                             having count (interest_id) > 6)

select count (interest_id) as interest_id_remove
from [fresh_segments.interest_metrics]
where interest_id in (select interest_id 
                      from cte_interest_remove);

-- 4.Does this decision make sense to remove these data points from a business perspective? Use an example where there are all 14 months present to a removed interest example for your arguments - think about what it means to have less months present from a segment perspective.


-- 5.After removing these interests - how many unique interests are there for each month?
>>>>>>> 8d149e582325633972de69a092410b255b52418d
