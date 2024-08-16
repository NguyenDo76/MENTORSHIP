-- A.DATA EXPLORATION AND CLEANSING
-- 1.Update the fresh_segments.interest_metrics table by modifying the month_year column to be a date data type with the start of the month
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
full join [dbo].[fresh_segments.interest_map] ma on me.interest_id = ma.id;

-- 5.Summarise the id values in the fresh_segments.interest_map by its total record count in this table
select count (*) as count_id
from [dbo].[fresh_segments.interest_map];

-- 6.What sort of table join should we perform for our analysis and why? Check your logic by checking the rows where interest_id = 21246 in your joined output and include all columns from fresh_segments.interest_metrics and all columns from fresh_segments.interest_map except from the id column.
select me.*, interest_name, interest_summary, created_at, last_modified
from [dbo].[fresh_segments.interest_metrics] me
join [dbo].[fresh_segments.interest_map] ma on ma.id = me.interest_id
where interest_id = '21246';

-- 7.Are there any records in your joined table where the month_year value is before the created_at value from the fresh_segments.interest_map table? Do you think these values are valid and why?
select count (interest_id) as count_id
from [dbo].[fresh_segments.interest_metrics] me
join [dbo].[fresh_segments.interest_map] ma on ma.id = me.interest_id
where month_year < created_at;
--> 188 id

select count (interest_id) as count_id
from [dbo].[fresh_segments.interest_metrics] me
join [dbo].[fresh_segments.interest_map] ma on ma.id = me.interest_id
where datetrunc (month, month_year) < datetrunc (month, created_at);
--> moth_year is the first day in a month so we use datetrunc to compare with month -> result is 0 id


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
-- Base on the total_month, we remove month < 6 

with cte_interest_remove as (select interest_id,
                                    count (interest_id) as count_month
                             from [fresh_segments.interest_metrics]
                             group by interest_id
                             having count (interest_id) >= 6)

select count (interest_id) as interest_id_remove
from [fresh_segments.interest_metrics]
where interest_id not in (select interest_id 
                      from cte_interest_remove);

-- 4.Does this decision make sense to remove these data points from a business perspective? Use an example where there are all 14 months present to a removed interest example for your arguments - think about what it means to have less months present from a segment perspective.


-- 5.After removing these interests - how many unique interests are there for each month?


-- C.SEGMENT ANALYSIS
-- 1.Using our filtered dataset by removing the interests with less than 6 months worth of data, which are the top 10 and bottom 10 interests which have the largest composition values in any month_year? Only use the maximum composition value for each interest but you must keep the corresponding month_year
--Create table has filtered dataset by removing the interests with less than 6 months worth of data
with cte_interest_remove as (select interest_id,
                                    count (interest_id) as count_month
                             from [fresh_segments.interest_metrics]
                             group by interest_id
                             having count (interest_id) < 6)
select *
into ##filtered_interest
from [fresh_segments.interest_metrics]
where interest_id not in (select interest_id from cte_interest_remove);

--TOP 10
with cte_interest_removed as (select interest_id, month_year,
                                     max (composition) as max_composition,
                                     dense_rank () over (partition by interest_id order by max (composition) desc) as rank_composition
                              from ##filtered_interest
                              where month_year is not null
                              group by interest_id, month_year)

select month_year, interest_name, max_composition
from cte_interest_removed cte
join [fresh_segments.interest_map] m on m.id = cte.interest_id
where rank_composition <= 10
order by month_year, max_composition desc;
                                          
--BOTTOM 10
with cte_interest_removed as (select interest_id, month_year,
                                     max (composition) as max_composition,
                                     dense_rank () over (partition by interest_id order by max (composition)) as rank_composition
                              from ##filtered_interest
                              where month_year is not null
                              group by interest_id, month_year)


select month_year, interest_name, max_composition
from cte_interest_removed cte
join [fresh_segments.interest_map] m on m.id = cte.interest_id
where rank_composition < 10
order by month_year, max_composition;

-- 2.Which 5 interests had the lowest average ranking value?
with cte_rank_avg_ranking_by_interest_id as (select interest_id,
                                                    avg (ranking) as avg_ranking,
                                                    row_number () over (order by avg (ranking) asc) as rank_avg_ranking_by_interest_id
                                             from ##filtered_interest
                                             group by interest_id)

select interest_name, avg_ranking
from cte_rank_avg_ranking_by_interest_id cte
join [fresh_segments.interest_map] m on m.id = cte.interest_id
where rank_avg_ranking_by_interest_id <= 5
order by 2 asc;

-- 3.Which 5 interests had the largest standard deviation in their percentile_ranking value?
with cte_rank_std_percentile_ranking_by_interest_id as (select interest_id,
                                                                   round (stdev (percentile_ranking),2) as std_percentile_ranking,
                                                                   row_number () over (order by stdev (percentile_ranking) desc) as rank_std_percentile_ranking_by_interest_id
                                                            from ##filtered_interest
                                                            group by interest_id)

select interest_name, std_percentile_ranking
from cte_rank_std_percentile_ranking_by_interest_id cte
join [fresh_segments.interest_map] m on m.id = cte.interest_id
where rank_std_percentile_ranking_by_interest_id <= 5
order by 2 desc;


-- 4.For the 5 interests found in the previous question - what was minimum and maximum percentile_ranking values for each interest and its corresponding year_month value? Can you describe what is happening for these 5 interests?
with cte_rank_std_percentile_ranking_by_interest_id as (select interest_id,
                                                                   round (stdev (percentile_ranking),2) as std_percentile_ranking,
                                                                   row_number () over (order by stdev (percentile_ranking) desc) as rank_std_percentile_ranking_by_interest_id
                                                            from ##filtered_interest
                                                            group by interest_id),

     cte_max_min as (select month_year, interest_id, percentile_ranking,
                            row_number () over (partition by interest_id order by percentile_ranking desc) as max_percentile_ranking,
                            row_number () over (partition by interest_id order by percentile_ranking asc) as min_percentile_ranking
                     from [fresh_segments.interest_metrics] me
                     where interest_id in (select interest_id
                                           from cte_rank_std_percentile_ranking_by_interest_id
                                           where rank_std_percentile_ranking_by_interest_id <= 5))

select month_year, cast (interest_name as varchar (50)) as interest_name, percentile_ranking 
from cte_max_min cte
join [fresh_segments.interest_map] m on m.id = cte.interest_id
where max_percentile_ranking = 1 or min_percentile_ranking = 1
order by cast (interest_name as varchar (50)), percentile_ranking desc;

-- 5.How would you describe our customers in this segment based off their composition and ranking values? What sort of products or services should we show to these customers and what should we avoid?


-- D.INDEX ANALYSIS
-- The index_value is a measure which can be used to reverse calculate the average composition for Fresh Segments' clients. Average composition can be calculated by dividing the composition column by the index_value column rounded to 2 decimal places.
select [month],
       [year],
       month_year,
       interest_id,
       composition,
       round (composition / index_value, 2) as avg_composition,
       index_value,
       ranking,
       percentile_ranking
into ##new_interest_metrics
from [fresh_segments.interest_metrics];

-- 1.What is the top 10 interests by the average composition for each month?
with cte_rank_avg_composition as (select month_year,
                                         interest_id,
                                         avg_composition,
                                         row_number () over (partition by month_year order by avg_composition desc) as rank_avg_composition
                                  from ##new_interest_metrics me
                                  where [month] is not null)

select month_year,
       interest_name,
       avg_composition
from cte_rank_avg_composition cte
join [fresh_segments.interest_map] m on m.id = cte.interest_id
where rank_avg_composition <= 10;

-- 2.For all of these top 10 interests - which interest appears the most often?
with cte_rank_avg_composition as (select month_year,
                                         interest_id,
                                         avg_composition,
                                         row_number () over (partition by month_year order by avg_composition desc) as rank_avg_composition
                                  from ##new_interest_metrics me
                                  where [month] is not null),

     cte_rank_interest_appears as (select interest_id,
                                          count (interest_id) as count_interest,
                                          rank () over (order by count (interest_id) desc) as rank_interest_appears
                                   from cte_rank_avg_composition
                                   where rank_avg_composition <= 10
                                   group by interest_id)

select interest_name, count_interest
from cte_rank_interest_appears cte
join [fresh_segments.interest_map] m on m.id = cte.interest_id
where rank_interest_appears = 1;

-- 3.What is the average of the average composition for the top 10 interests for each month?
with cte_rank_avg_composition as (select month_year,
                                         interest_id,
                                         avg_composition,
                                         row_number () over (partition by month_year order by avg_composition desc) as rank_avg_composition
                                  from ##new_interest_metrics me
                                  where [month] is not null)

select month_year,
       round (avg (avg_composition),2) as avg_of_avg_composition
from cte_rank_avg_composition cte
join [fresh_segments.interest_map] m on m.id = cte.interest_id 
where rank_avg_composition <= 10
group by month_year
order by month_year;