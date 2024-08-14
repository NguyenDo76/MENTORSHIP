--A.DATA CLEANING
drop table if exists ##clean_weekly_sales
select week_date,
       datepart (week,week_date) as week_number,
       month (week_date) as month_number,
       year (week_date) as calendar_year,
       region, platform, segment,
       case when right (segment, 1) = '1' then 'Young Adults'
            when right (segment,1) = '2' then 'Middle Aged' 
            when right (segment,1) in ('3','4') then 'Retirees'else 'Unknow' end as age_band,
       case when left (segment, 1) = 'C' then 'Couples'
            when left (segment, 1) = 'F' then 'Families' else 'Unknow' end as demographic,
       customer_type, transactions,
       round (sales/transactions,2) as avg_transaction,
       sales
into ##clean_weekly_sales
from data_mart;

--B.DATA EXPLORATION:
-- 1.What day of the week is used for each week_date value?
select distinct datename (weekday,week_date) as week_day
from ##clean_weekly_sales;

-- 2.What range of week numbers are missing from the dataset?
with cte_week_number as (select value as week_number_of_year 
                         from generate_series (1,52))

select w.week_number_of_year
from cte_week_number w
left join ##clean_weekly_sales c on c.week_number = w.week_number_of_year
where c.week_number is null
order by w.week_number_of_year;

-- 3.How many total transactions were there for each year in the dataset?
select calender_year,
       sum (transactions) as total_transactions
from ##clean_weekly_sales
group by calender_year
order by calender_year;

-- 4.What is the total sales for each region for each month?
select month_number, region,
       sum (cast (sales as float)) as total_sales
from ##clean_weekly_sales
group by month_number, region
order by month_number, region;

-- 5.What is the total count of transactions for each platform
select platform,
       count (transactions) as count_transaction 
from ##clean_weekly_sales
group by platform;

-- 6.What is the percentage of sales for Retail vs Shopify for each month?
select calendar_year, month_number,
       round (100 * sum (case when platform = 'Retail' then cast (sales as float) end) / sum (cast (sales as float)),2) as percentage_sales_retail,
       round (100 * sum (case when platform = 'Shopify' then cast (sales as float) end) / sum (cast (sales as float)),2) as percentage_sales_shopify
from ##clean_weekly_sales
group by calendar_year, month_number
order by calendar_year, month_number;

-- 7.What is the percentage of sales by demographic for each year in the dataset?
select calendar_year,
       round (100 * sum (case when demographic = 'Couples' then cast (sales as float) end) / sum (cast (sales as float)),2) as percentage_sales_couples,
       round (100 * sum (case when demographic = 'Families' then cast (sales as float) end) / sum (cast (sales as float)),2) as percentage_sales_families,
        round (100 * sum (case when demographic = 'Unknow' then cast (sales as float) end) / sum (cast (sales as float)),2) as percentage_sales_unknow
from ##clean_weekly_sales
group by calendar_year
order by calendar_year;

-- 8.Which age_band and demographic values contribute the most to Retail sales?
select age_band, demographic,
       sum (cast (sales as float)) as total_sales,
       round (100 * sum (cast (sales as float))/(select sum (cast (sales as float)) from ##clean_weekly_sales where platform = 'Retail'),2) as percentage
from ##clean_weekly_sales
where platform = 'Retail'
group by age_band, demographic
order by total_sales desc, age_band, demographic;

-- 9.Can we use the avg_transaction column to find the average transaction size for each year for Retail vs Shopify? If not - how would you calculate it instead?
select calendar_year, platform,
       avg (avg_transaction) as total_avg_trans_row,
       round (sum (cast (sales as float)) / sum (transactions) , 2) as avg_transactions
from ##clean_weekly_sales
group by calendar_year, platform
order by calendar_year, platform;

--C.BEFORE & AFTER ANALYSIS
select distinct (week_number)
from ##clean_weekly_sales
where week_date = '2020-06-15';

-- 1.What is the total sales for the 4 weeks before and after 2020-06-15? What is the growth or reduction rate in actual values and percentage of sales?
with cte_week_sales as (select week_date, week_number, sales
                        from ##clean_weekly_sales
                        where calendar_year = 2020 and week_number between (25-4) and (25+3)),

     cte_summations as (Select sum (case when week_number between (25-4) and (25-1) then cast (sales as float) end) as total_sales_before,
                               sum (case when week_number  between 25 and (25+3) then cast (sales as float) end) as total_sales_after 
                        from cte_week_sales)

select total_sales_before, total_sales_after,
       total_sales_after - total_sales_before as diff,
       round (100 * (total_sales_after - total_sales_before)/total_sales_before,2) as percentage
from cte_summations;

-- 2.What about the entire 12 weeks before and after?
with cte_week_sales as (select week_date, week_number, sales
                        from ##clean_weekly_sales
                        where calendar_year = 2020 and week_number between (25-12) and (25+11)),

     cte_summations as (Select sum (case when week_number between (25-12) and (25-1) then cast (sales as float) end) as total_sales_before,
                               sum (case when week_number  between 25 and (25+11) then cast (sales as float) end) as total_sales_after 
                        from cte_week_sales)

select total_sales_before, total_sales_after,
       total_sales_after - total_sales_before as diff,
       round (100 * (total_sales_after - total_sales_before)/total_sales_before,2) as percentage
from cte_summations;

-- 3.How do the sale metrics for these 2 periods before and after compare with the previous years in 2018 and 2019?
--4 week
with cte_week_sales as (select week_date, week_number, sales,calendar_year
                        from ##clean_weekly_sales
                        where calendar_year != 2020 and week_number between (25-4) and (25+3)),

     cte_summations as (Select calendar_year,
                               sum (case when week_number between (25-4) and (25-1) then cast (sales as float) end) as total_sales_before,
                               sum (case when week_number  between 25 and (25+3) then cast (sales as float) end) as total_sales_after 
                        from cte_week_sales
                        group by calendar_year)

select calendar_year,
       total_sales_before, total_sales_after,
       total_sales_after - total_sales_before as diff,
       round (100 * (total_sales_after - total_sales_before)/total_sales_before,2) as percentage
from cte_summations
order by calendar_year;

--12 week
with cte_week_sales as (select week_date, week_number, sales, calendar_year
                        from ##clean_weekly_sales
                        where calendar_year != 2020 and week_number between (25-12) and (25+11)),

     cte_summations as (Select calendar_year,
                               sum (case when week_number between (25-12) and (25-1) then cast (sales as float) end) as total_sales_before,
                               sum (case when week_number  between 25 and (25+11) then cast (sales as float) end) as total_sales_after 
                        from cte_week_sales
                        group by calendar_year)

select calendar_year,
       total_sales_before, total_sales_after,
       total_sales_after - total_sales_before as diff,
       round (100 * (total_sales_after - total_sales_before)/total_sales_before,2) as percentage
from cte_summations
order by calendar_year;
