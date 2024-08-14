--DIGITAL ANALYSIS
-- 1.How many users are there?
select count (distinct user_id) as count_users
from [clique_bait.users];

-- 2.How many cookies does each user have on average?
with cte_count_cookie as (select user_id,
                                 count (cookie_id) as count_cookie
                          from [clique_bait.users]
                          group by user_id)

select avg (count_cookie) as avg_cookie
from cte_count_cookie;

-- 3.What is the unique number of visits by all users per month?
select datepart (month, event_time) as [month],
       count (distinct visit_id) as count_visit
from [clique_bait.events]
group by datepart (month, event_time)
order by datepart (month, event_time);

-- 4.What is the number of events for each event type?
select event_type,
       count (event_type) as count_events
from [clique_bait.events]
group by event_type
order by event_type;

-- 5.What is the percentage of visits which have a purchase event?
select 100 * count (distinct visit_id)/
       (select count (distinct visit_id) from [clique_bait.events]) as percentage_purchase
from [clique_bait.events] e
join [clique_bait.event_identifier] ei on ei.event_type = e.event_type
where event_name = 'purchase'

-- 6.What is the percentage of visits which view the checkout page but do not have a purchase event?
--My answer
select 100 * count (distinct visit_id)/
       (select count (distinct visit_id) from [clique_bait.events]) as percentage_purchase
from [clique_bait.events] e
join [clique_bait.event_identifier] ei on ei.event_type = e.event_type
join [clique_bait.page_hierarchy] h on h.page_id = e.page_id
where event_name != 'purchase' and page_name = 'checkout';
--github's answer
WITH checkout_purchase AS (
SELECT 
  visit_id,
  MAX(CASE WHEN event_type = 1 AND page_id = 12 THEN 1 ELSE 0 END) AS checkout,
  MAX(CASE WHEN event_type = 3 THEN 1 ELSE 0 END) AS purchase
FROM [clique_bait.events]
GROUP BY visit_id)

SELECT 
  ROUND(100 * (1-(SUM(purchase)/SUM(checkout))),2) AS percentage_checkout_view_with_no_purchase
FROM checkout_purchase;

-- 7.What are the top 3 pages by number of views?
select top (3) page_name,
               count (visit_id) as count_of_pageview
from [clique_bait.events] e
join [clique_bait.page_hierarchy] h on h.page_id = e.page_id
join [clique_bait.event_identifier] ei on ei.event_type = e.event_type
where event_name = 'pageview'
group by page_name
order by count (visit_id) desc;

-- 8.What is the number of views and cart adds for each product category?
select product_category,
       sum (case when event_name = 'Pageview' then 1 else 0 end) as count_pageview,
       sum (case when event_name = 'Addtocart' then 1 else 0 end) as count_addtocart
from [clique_bait.events] e
join [clique_bait.page_hierarchy] h on h.page_id = e.page_id
join [clique_bait.event_identifier] ei on ei.event_type = e.event_type
group by product_category
order by product_category;

-- 9.What are the top 3 products by purchases?
select page_name, 
       count (visit_id) as count_purchase
from [clique_bait.events] e
join [clique_bait.page_hierarchy] h on e.page_id = h.page_id
where event_type = 3
group by page_name
order by count (visit_id) desc;
-------------------------------------------
with cte_purchase as (select visit_id, e.page_id, product_category, event_name
             from [clique_bait.events] e
             join [clique_bait.page_hierarchy] h on h.page_id = e.page_id
             join [clique_bait.event_identifier] ei on ei.event_type = e.event_type
             where visit_id in (select visit_id
                                from [clique_bait.events]
                                where event_type = 3) -- purchase
                                order by visit_id, page_id) --list of visit_id to purchase

select top (3) page_name,
               count (cte.page_id) as count_product
from cte_purchase cte
join [clique_bait.page_hierarchy] h on h.page_id = cte.page_id
where cte.product_category is not null and event_name = 'addtocart' --addtocart to purcase
group by page_name
order by count (cte.page_id) desc;

--PRODUCT FUNNEL ANALYSIS
-- Using a single SQL query - create a new output table which has the following details:
--1.1.How many times was each product viewed?
--1.2.How many times was each product added to cart?
--1.3.How many times was each product added to a cart but not purchased (abandoned)?
--1.4.How many times was each product purchased?
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

-- Additionally, create another table which further aggregates the data for the above points but this time for each product category instead of individual products.
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

--Question:
-- 1.Which product had the most views, cart adds and purchases?
with cte_rank as (select product_name,
                         row_number () over (order by views desc) as rank_views,
                         row_number () over (order by addtocard desc) as rank_add,
                         row_number () over (order by purchase desc) as rank_purchase
                  from ##product)

select max (case when rank_views = 1 then product_name end) as product_most_views,
       max (case when rank_add = 1 then product_name end) as product_most_add,
       max (case when rank_purchase = 1 then product_name end) as product_most_purchase
from cte_rank;

-- 2.Which product was most likely to be abandoned?
--Ref1
with cte_rank as (select product_name,
                         round (100* purchase / addtocard,2) as percentage_abandoned,
                         dense_rank () over (order by round (100* purchase / addtocard,2)) as rank_percentage_abandoned
                  from ##product)

select *
from cte_rank
where rank_percentage_abandoned = 1;

--Ref 2
with cte_rank as (select product_name,
                         dense_rank () over (order by abandoned desc) as rank_abandoned 
                  from ##product)

select *
from cte_rank
where rank_abandoned = 1;

-- 3.Which product had the highest view to purchase percentage?
with cte_rank_percentage as (select  product_name,
                                     round (100 * purchase / views, 2) as [percentage],
                                     dense_rank () over (order by round (100 * purchase / views, 2) desc )as [rank_percentage]
                             from ##product)

select *
from cte_rank_percentage
where rank_percentage = 1;

-- 4.What is the average conversion rate from view to cart add?
select round (avg (100* addtocard / views),2) as avg_add_from_views
from ##product;

-- 5.What is the average conversion rate from cart add to purchase?
select round (avg (100* purchase / addtocard),2) as avg_add_from_views
from ##product;

--CAMPAIGNS ANALYSIS
-- Generate a table that has 1 single row for every unique visit_id record and has the following columns:
-- user_id
-- visit_id
-- visit_start_time: the earliest event_time for each visit
-- page_views: count of page views for each visit
-- cart_adds: count of product cart add events for each visit
-- purchase: 1/0 flag if a purchase event exists for each visit
-- campaign_name: map the visit to a campaign if the visit_start_time falls between the start_date and end_date
-- impression: count of ad impressions for each visit
-- click: count of ad clicks for each visit
-- (Optional column) cart_products: a comma separated text value with products added to the cart sorted by the order they were added to the cart (hint: use the sequence_number)
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














