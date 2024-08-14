-- A. Customer Nodes Exploration
-- 1.How many unique nodes are there on the Data Bank system?
select count (distinct node_id) as count_unique_nodes
from data_bank.customer_nodes;

-- 2.What is the number of nodes per region?
select c.region_id, region_name,
       count (distinct node_id) as count_nodes
from data_bank.customer_nodes c
join data_bank.regions r on r.region_id = c.region_id
group by c.region_id, region_name
order by c.region_id;

-- 3.How many customers are allocated to each region?
select c.region_id, region_name,
       count (customer_id) as count_customer
from data_bank.customer_nodes c
join data_bank.regions r on r.region_id = c.region_id
group by c.region_id, region_name
order by c.region_id;

-- 4.How many days on average are customers reallocated to a different node?
with cte_diff_day as (select customer_id, node_id, 
                    sum ((end_date - start_date)) as diff_day
             from data_bank.customer_nodes c
             where end_date != '9999-12-31'
             group by customer_id, node_id)

select round (avg (diff_day),0) as avg_customer
from cte_diff_day;

-- 5.What is the median, 80th and 95th percentile for this same reallocation days metric for each region?
with cte_diff_day as (select customer_id, node_id, 
                    sum ((end_date - start_date)) as diff_day
             from data_bank.customer_nodes c
             where end_date != '9999-12-31'
             group by customer_id, node_id)

select percentile_cont (0.5) within group (order by diff_day) as median,
       percentile_cont (0.8) within group (order by diff_day) as P80,
       percentile_cont (0.95) within group (order by diff_day) as P95
from cte_diff_day;


-- B. Customer Transactions
-- 1.What is the unique count and total amount for each transaction type?
select txn_type,
       count (distinct customer_id) as count_cus,
       sum (txn_amount) as total_amount
from data_bank.customer_transactions
group by txn_type;

-- 2.What is the average total historical deposit counts and amounts for all customers?
with cte_deposit as (select customer_id,
                    count (customer_id) as count_cus,
                    sum (txn_amount) as total_amount
             from data_bank.customer_transactions
             where txn_type = 'deposit'
             group by customer_id)

select round (avg (count_cus)) as avg_cus,
	round (avg (total_amount)) avg_amount
from cte_deposit;

-- 3.For each month - how many Data Bank customers make more than 1 deposit and either 1 purchase or 1 withdrawal in a single month?
with cte_total_type as (select customer_id,
                    date_part ('month',txn_date) as month,
                    sum (case when txn_type = 'deposit' then 1 else 0 end) as total_deposit,
                    sum (case when txn_type = 'purchase' then 1 else 0 end) as total_purchase,
                    sum (case when txn_type = 'withdrawal' then 1 else 0 end) as total_withdrawal
             from data_bank.customer_transactions
             group by customer_id, date_part ('month',txn_date))

select month,
       count (customer_id) as count_cus
from cte_total_type
where total_deposit > 1 and (total_purchase >= 1 or total_withdrawal >= 1)
group by month
order by month

-- 4.What is the closing balance for each customer at the end of the month? Also show the change in balance each month in the same table output.
with cte_total_type as (select customer_id,
                    date_part ('month',txn_date) as month,
                    sum (case when txn_type = 'deposit' then txn_amount else 0 end) as total_deposit,
                    sum (case when txn_type = 'purchase' then txn_amount else 0 end) as total_purchase,
                    sum (case when txn_type = 'withdrawal' then txn_amount else 0 end) as total_withdrawal
             from data_bank.customer_transactions
             group by customer_id, date_part ('month',txn_date)),

     cte_balance_in_month as (select customer_id, month,
                     total_deposit - total_purchase - total_withdrawal as balance_in_month
              from cte_total_type),

     cte_closing_balance as (select customer_id, month,
                     sum (balance_in_month) over (partition by customer_id order by month) AS closing_balance
              from cte_balance_in_month)

select customer_id, month,
       coalesce (closing_balance, 0) as closing_balance,
       coalesce (closing_balance - lag (closing_balance,1) over (partition by customer_id order by month),0) as change_in_blance
from cte_closing_balance
order by customer_id, month
