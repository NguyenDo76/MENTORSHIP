--CLEANING AND TRANFROM DATA
select order_id, customer_id, pizza_id,
       case when exclusions = 'null' then '' else exclusions end as exclusions,
       case when extras = 'null' or extras is null then '' else extras end as extras,
       order_time
into ##new_customer_orders
from customer_orders

select order_id, runner_id,
       case when pickup_time = 'null' then '' else pickup_time end as pickup_time,
       case when distance = 'null' then ''
            when distance like '%km' then trim ('km' from distance) 
            else distance
       end as distance,
       case when duration = 'null' then '' 
            when duration like '%nu%' then trim ('minutes' from duration)
            when duration like '%mi%' then trim ('mins' from duration)
            else duration
       end as duration,
       case when cancellation = 'null' or cancellation is null then  '' else cancellation end as cancellation
into ##new_runner_orders
from runner_orders