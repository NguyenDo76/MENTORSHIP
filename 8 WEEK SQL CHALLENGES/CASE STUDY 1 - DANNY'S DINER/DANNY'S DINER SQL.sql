-- 1. What is the total amount each customer spent at the restaurant?
select s.customer_id,
	    sum (m.price) as total_price
from sales s
join menu m on m.product_id = s.product_id
group by s.customer_id
order by total_price DESC;

-- 2. How many days has each customer visited the restaurant?
select s.customer_id,
        count (distinct s.order_date) as count_day
From sales s
group by s.customer_id
order by s.customer_id asc

-- 3. What was the first item from the menu purchased by each customer?
with cte as (select s.customer_id, m.product_name,
                    dense_rank () over (partition by s.customer_id order by s.order_date asc) as "rank_date"
            from sales s
            join menu m on m.product_id = s.product_id)

select customer_id, product_name
from cte
where rank_date = 1
group by customer_id, product_name

-- 4. What is the most purchased item on the menu and how many times was it purchased by all customers?
select top (1) m.product_name,
                count (s.product_id) as count_product
from sales s
join menu m on s.product_id = m.product_id
group by m.product_name
order by count_product DESC

-- 5. Which item was the most popular for each customer?
with cte as (select customer_id, m.product_name,  COUNT(s.product_id) as count_product,
                    dense_rank () over (partition by customer_id order by COUNT(s.product_id) desc) as rank_product
            from sales s
            join menu m on s.product_id = m.product_id
            group by customer_id, m.product_name)

select customer_id, product_name, count_product
from cte
where rank_product = 1

-- 6. Which item was purchased first by the customer after they became a member?

with a as (select s.customer_id, s.product_id,
                datediff (DY,mb.join_date,s.order_date) as count_date
            from sales s
            join members mb on mb.customer_id = s.customer_id),

    b as (select customer_id, product_id,
                row_number () over (partition by customer_id order by count_date asc) as 'rank'
            from a
            where count_date > 0)

SELECT customer_id, b.product_id, product_name
from b
join menu m on b.product_id = m.product_id
where rank = 1

-- 7. Which item was purchased just before the customer became a member?
with a as (select s.customer_id, s.product_id,
                DATEDIFF(DY,mb.join_date,s.order_date) as count_date
            from sales s
            join members mb on mb.customer_id = s.customer_id),

    b as (select customer_id, product_id,
                row_number () over (partition by customer_id order by count_date desc) as 'rank'
            from a
            where count_date < 0)

SELECT customer_id, b.product_id, product_name
from b
join menu m on b.product_id = m.product_id
where rank = 1

-- 8. What is the total items and amount spent for each member before they became a member?
select s.customer_id, 
        count (s.product_id) as total_items,
        sum (price) as total_price
from sales s 
join members mb on mb.customer_id = s.customer_id
join menu m on m.product_id = s.product_id
where mb.join_date > s.order_date
group by s.customer_id

-- 9. If each $1 spent equates to 10 points and sushi has a 2x points multiplier - how many points would each customer have?
with a as (select s.customer_id,
                case when m.product_name = 'sushi' then m.price * 10 * 2 else m.price *10 end as total_points
            from sales s 
            join menu m on m.product_id = s.product_id)

select customer_id, sum (total_points) as total_point_by_customer
from a
group by customer_id

-- 10. In the first week after a customer joins the program (including their join date) they earn 2x points on all items, not just sushi - how many points do customer A and B have at the end of January?

with cte as (select s.customer_id,
CASE WHEN DATEDIFF (DY,mb.join_date,s.order_date) BETWEEN 0 and 6 then price *10 * 2
    WHEN DATEDIFF (DY,mb.join_date,s.order_date) < 0 and m.product_name = 'sushi' then m.price * 10 * 2 
    WHEN DATEDIFF (DY,mb.join_date,s.order_date) >= 7 and m.product_name = 'sushi' then m.price * 10 * 2 
else price *10
end as total_points
from sales s
join menu m on m.product_id = s.product_id
join members mb on mb.customer_id = s.customer_id
where MONTH(s.order_date) = 1)

select customer_id, 
        sum (total_points) as total_point_by_customer_in_January
from cte 
group by customer_id
order by customer_id

--Bonus Questions
-- Join All The Things
-- Recreate the following table output using the available data

select s.customer_id, s.order_date, m.product_name, m.price,
        case when datediff (dy,mb.join_date,s.order_date) >=  0 then 'Y' else 'N' end as member
from sales s
left join menu m on m.product_id = s.product_id
left join members mb on mb.customer_id = s.customer_id

-- Rank All The Things
-- Danny also requires further information about the ranking of customer products, 
-- but he purposely does not need the ranking for non-member purchases 
-- so he expects null ranking values for the records when customers are not yet part of the loyalty program.

with cte as (select s.customer_id, s.order_date, m.product_name, m.price,
                    case when datediff (dy,mb.join_date,s.order_date) >=  0 then 'Y' else 'N' end as member
            from sales s
            left join menu m on m.product_id = s.product_id
            left join members mb on mb.customer_id = s.customer_id)

select *,
case when member = 'Y' then dense_rank () over (partition by customer_id, member order by order_date asc) end as ranking
from cte