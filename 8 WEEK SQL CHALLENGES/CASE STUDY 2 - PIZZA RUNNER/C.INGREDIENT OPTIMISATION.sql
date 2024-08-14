-- C. Ingredient Optimisation
-- CLEANING AND TRANFROM DATA
drop table if exists ##new_pizza_recipes
select pizza_id, a.topping_id, topping_name
into ##new_pizza_recipes
from (select pizza_id, 
             cast (value as int) as topping_id
      from pizza_recipes r
      cross apply string_split (cast (toppings as varchar (50)),',')) a
join pizza_toppings t on t.topping_id = a.topping_id;

drop table if exists ##new_extras
select row_number () over (order by order_id asc) as record_id,order_id, customer_id, pizza_id, cast (value as int) as extras
into ##new_extras
from ##new_customer_orders
cross apply string_split(cast (extras as varchar (50)),',')
where extras != '';

drop table if exists ##new_exclusions
select row_number () over (order by order_id asc) as record_id,order_id, customer_id, pizza_id, cast (value as int) as exclusions
into ##new_exclusions
from ##new_customer_orders
cross apply string_split(cast (exclusions as varchar (50)),',')
where exclusions != '';

-- 1.What are the standard ingredients for each pizza?
select pizza_id, 
       string_agg (cast (topping_name as varchar (50)),',') as topping_name
from ##new_pizza_recipes
group by pizza_id;

-- 2.What was the most commonly added extra?
select t.topping_id,
       cast (topping_name as varchar (50)) as topping_name,
       count (extras) as count_topping_added
from ##new_extras e
join pizza_toppings t on t.topping_id = e.extras
group by t.topping_id, cast (topping_name as varchar (50));

-- 3.What was the most common exclusion?
select t.topping_id,
       cast (topping_name as varchar (50)) as topping_name,
       count (exclusions) as count_topping_exclusions
from ##new_exclusions ex
join pizza_toppings t on t.topping_id = ex.exclusions
group by t.topping_id, cast (topping_name as varchar (50));