-- HIGH LEVEL SALES ANALYSIS
-- 1.What was the total quantity sold for all products?
select sum (qty) as total_product
from [balanced_tree.sales];

-- 2.What is the total generated revenue for all products before discounts?
select sum (qty* price) as revenue_before_discount
from [dbo].[balanced_tree.sales];

-- 3.What was the total discount amount for all products?
select sum (qty * price * discount/100) as total_discount
from [dbo].[balanced_tree.sales];

-- TRANSACTION ANALYSIS
-- 1.How many unique transactions were there?
select count (distinct txn_id) as count_unique_trans 
from [dbo].[balanced_tree.sales];

-- 2.What is the average 'unique products' purchased in each transaction?
-- My question
with cte_prod as (select txn_id,
                         count (prod_id) as count_prod,
                         sum (qty) as total_prod
                  from [dbo].[balanced_tree.sales]
                  group by txn_id)

select 
       sum (total_prod) / sum (count_prod) as avg_prod
from cte_prod;
-- Mentor question
with cte_prod as (select txn_id,
                         sum (qty) as total_prod
                  from [dbo].[balanced_tree.sales]
                  group by txn_id)

select avg (total_prod) as avg_prod
from cte_prod;
-- Web question
with cte_prod as (select txn_id,
                         count (prod_id) as count_prod
                  from [dbo].[balanced_tree.sales]
                  group by txn_id)

select avg(count_prod) as avg_prod
from cte_prod;

-- 3.What are the 25th, 50th and 75th percentile values for the revenue per transaction?
with cte_revenue as (select txn_id,
                            sum (qty * price) as revenue
                     from [dbo].[balanced_tree.sales]
                     group by txn_id)

select distinct percentile_cont (0.25) within group (order by revenue) over () as P25,
                percentile_cont (0.5) within group (order by revenue) over () as median,
                percentile_cont (0.75) within group (order by revenue) over () as P75
from cte_revenue;

-- 4.What is the average discount value per transaction?
with cte_discount as (select txn_id,
                             sum (qty * price * discount/100) as total_discount
                      from [balanced_tree.sales]
                      group by txn_id)

select avg (total_discount) as avg_discount
from cte_discount;

-- 5.What is the percentage split of all transactions for members vs non-members?
with cte_member as (select distinct txn_id,
                                    case when member = 't' then 'member' else 'non-member' end as member
                    from [balanced_tree.sales])

select member,
       count (member) as count_member,
       cast (round (count (member) * 100.0 / sum (count (member)) over (),2) as float) as percentage_member
from cte_member
group by member;

-- 6.What is the average revenue for member transactions and non-member transactions?
with cte_member as (select txn_id,
                           case when member = 't' then 'member' else 'non-member' end as member,
                           sum (qty * price) as revenue
                    from [balanced_tree.sales]
                    group by txn_id, member)

select member,
       avg (revenue) as avg_revenue
from cte_member
group by member;

-- PRODUCT ANALYSIS
-- 1.What are the top 3 products by total revenue before discount?
select top (3) d.product_name,
               sum (qty * s.price) as total_revenue
from [balanced_tree.sales] s
join [dbo].[balanced_tree.product_details] d on s.prod_id = d.product_id
group by d.product_name
order by sum (qty * s.price) desc;

-- 2.What is the total quantity, revenue and discount for each segment?
select segment_id, segment_name,
       sum (qty) as total_qty,
       sum (qty * s.price) as total_reevenue,
       sum (qty * s.price * discount/100) as total_discount
from [balanced_tree.sales] s
join [dbo].[balanced_tree.product_details] d on s.prod_id = d.product_id
group by segment_id, segment_name
order by segment_id;

-- 3.What is the top selling product for each segment?
with cte_rank as (select segment_id, segment_name, product_id, product_name,
                         sum (qty) as total_qty,
                         dense_rank () over (partition by segment_id order by sum (qty) desc) as rank
                  from [balanced_tree.sales] s
                  join [dbo].[balanced_tree.product_details] d on s.prod_id = d.product_id
                  group by segment_id, segment_name, product_id, product_name)

select segment_id, segment_name, product_id, product_name, total_qty
from cte_rank
where rank = 1
order by segment_id, product_id;

-- 4.What is the total quantity, revenue and discount for each category?
select category_id, category_name,
       sum (qty) as total_qty,
       sum (qty * s.price) as total_reevenue,
       sum (qty * s.price * discount/100) as total_discount
from [balanced_tree.sales] s
join [dbo].[balanced_tree.product_details] d on s.prod_id = d.product_id
group by category_id, category_name
order by category_id;

-- 5.What is the top selling product for each category?
with cte_rank as (select category_id, category_name, product_id, product_name,
                         sum (qty) as total_qty,
                         dense_rank () over (partition by category_id order by sum (qty) desc) as rank
                  from [balanced_tree.sales] s
                  join [dbo].[balanced_tree.product_details] d on s.prod_id = d.product_id
                  group by category_id, category_name, product_id, product_name)

select category_id, category_name, product_id, product_name, total_qty
from cte_rank
where rank = 1
order by category_id, product_id;

-- 6.What is the percentage split of revenue by product for each segment?
with cte_revennue as (select segment_id, segment_name, product_id, product_name,
                             sum (qty * s.price) as revenue
                      from [balanced_tree.sales] s
                      join [dbo].[balanced_tree.product_details] d on s.prod_id = d.product_id
                      group by segment_id, segment_name, product_id, product_name)

select segment_id, segment_name, product_id, product_name,
       cast (round (100.0 * revenue / sum (revenue) over (partition by segment_id),2) as float) as [percentage]
from cte_revennue
order by segment_id;

-- 7.What is the percentage split of revenue by segment for each category?
with cte_revennue as (select category_id, category_name, segment_id, segment_name,
                             sum (qty * s.price) as revenue
                      from [balanced_tree.sales] s
                      join [dbo].[balanced_tree.product_details] d on s.prod_id = d.product_id
                      group by category_id, category_name, segment_id, segment_name)

select category_id, category_name, segment_id, segment_name,
       cast (round (100.0 * revenue / sum (revenue) over (partition by category_id),2) as float) as [percentage]
from cte_revennue;

-- 8.What is the percentage split of total revenue by category?
with cte_revennue as (select category_id, category_name,
                             sum (qty * s.price) as revenue
                      from [balanced_tree.sales] s
                      join [dbo].[balanced_tree.product_details] d on s.prod_id = d.product_id
                      group by category_id, category_name)

select category_id, category_name,
       cast (round (100.0 * revenue / sum (revenue) over (),2) as float) as [percentage]
from cte_revennue;

-- 9.What is the total transaction 'penetration' for each product? (hint: penetration = number of transactions where at least 1 quantity of a product was purchased divided by total number of transactions)
-- with cte_qty as (select txn_id,prod_id,
--                                 sum (qty) as total_product
--                  from [balanced_tree.sales]
--                  group by txn_id,prod_id)

-- select txn_id, prod_id,
--        round (100 * count (prod_id) / count (distinct txn_id),2)
-- from cte_qty
-- group by txn_id,prod_id;

-- 10.What is the most common combination of at least 1 quantity of any 3 products in a 1 single transaction?
