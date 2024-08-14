
-- The color table contains the following columns:
-- 	    idstores the unique ID for each color.
-- 	    name stores the name of the color.
-- 	    extra_fee stores the extra charge (if any) added for clothing ordered in this color.
	
-- In the customer table, you'll find the following columns:
-- 	    id stores customer IDs.
-- 	    first_name stores the customer's first name.
--  	last_name stores the customer's last name.
-- 	    favorite_color_idstores the ID of the customer's favorite color (references the color table).

-- The category table contains these columns:
-- 	    id stores the unique ID for each category.
-- 	    name stores the name of the category.
--  	parent_id stores the ID of the main category for this category (if it's a subcategory). If this value is NULL, it denotes that this category is a main category. Note: Values are related to those in the id column in this table.

-- The clothing table stores data in the following columns:
--  	id stores the unique ID for each item.
-- 	    name stores the name of that item.
--  	size stores the size of that clothing: S, M, L, XL, 2XL, or 3XL.
-- 	    price stores the item's price.
-- 	    color_id stores the item's color (references the color table).
-- 	    category_id stores the item's category (references the category table).

-- The clothing_order table contains the following columns:
-- 	    id stores the unique order ID.
-- 	    customer_id stores the ID of the customer ordering the clothes (references the customer table).
-- 	    clothing_id stores the ID of the item ordered (references the clothing table).
-- 	    items stores how many of that clothing item the customer ordered.
-- 	    order_date stores the date of the order.

--QUESTION:

--1.Create Tables
drop table if exists session_1_color;
create table session_1_color (
		id int,
		name varchar (50),
		extra_fee decimal (10,2) NULL
		);
insert into session_1_color values
    ('1','blue','0.1'),
    ('2','red','0.15'),
    ('3','black',NULL);


drop table if exists session_1_customer;
create table session_1_customer (
		id int,
		first_name varchar (50),
		last_name varchar (50),
		favorite_color_idstores int
		);
insert into session_1_customer values
    ('1','Cheryl','Stark','1'),
    ('2','Jay','Harber','2'),
    ('3','Jaime','Bauch','3'),
    ('4','Jackie','Hand','2'),
    ('5','Elijah','Kiehn','3');


drop table if exists session_1_category;
create table session_1_category (
		id int,
		name varchar (50),
		parent_id int
		);
insert into session_1_category values
    ('1','men',NULL),
    ('2','woman',NULL),
    ('3','shirt','1'),
    ('4','trousers','1'),
    ('5','dress','2');


drop table if exists session_1_clothing;
create table session_1_clothing (
		id int,
		name varchar (50),
		size varchar (10),
		price decimal (10,2),
		color_id int,
		category_id int
		);
insert into session_1_clothing values
    ('1','Black Trouser','S','15.5','3','4'),
    ('2','Black Trouser','M','15.5','3','4'),
    ('3','Black Trouser','L','15.5','3','4'),
    ('4','Black Trouser','XL','15.5','3','4'),
    ('5','Black Trouser','2XL','15.5','3','4'),
    ('6','Red Shirt','S','13','2','3'),
    ('7','Red Shirt','M','13','2','3'),
    ('8','Red Shirt','L','13','2','3'),
    ('9','Red Shirt','XL','13','2','3'),
    ('10','Blue Dress','S','17.8','1','5'),
    ('11','Blue Dress','M','17.8','1','5'),
    ('12','Blue Dress','L','17.8','1','5'),
    ('13','Blue Dress','XL','17.8','1','5'),
    ('14','Blue Dress','2XL','17.8','1','5'),
    ('15','Blue Dress','3XL','17.8','1','5');


drop table if exists session_1_clothing_order;
create table session_1_clothing_order (
		id int,
		customer_id int,
		clothing_id int,
		items int,
		order_date datetime
		);
insert into session_1_clothing_order values
    ('1','2','1','2','2024-07-01'),
    ('2','2','3','5','2024-07-01'),
    ('3','3','2','3','2024-07-01'),
    ('4','4','5','1','2024-07-02'),
    ('5','5','8','7','2024-07-02'),
    ('6','4','10','3','2024-07-03'),
    ('7','2','2','5','2024-07-04'),
    ('8','3','13','8','2024-07-05');

--2.List All Clothing Items: Display the name of clothing items (name the column clothes), their color (name the column color),and the last name and first name of the customer(s) who bought this apparel in their favorite color. Sort rows according to color, in ascending order.

select distinct c.name,
                co1.name as color,
                last_name, first_name,
                co2.name as favorite_color
from session_1_clothing_order o
join session_1_clothing c on o.clothing_id = c.id
join session_1_color co1 on c.color_id = co1.id
join session_1_customer cu on o.customer_id = cu.id
join session_1_color co2 on co2.id = cu.favorite_color_idstores
order by co1.name;

--3.Get All Non-Buying Customers. Select the last name and first name of customers and the name of their favorite color for customers with no purchases.

select last_name, first_name, co.name
from session_1_clothing_order o
right join session_1_customer cu on cu.id = o.customer_id
join session_1_color co on co.id = cu.favorite_color_idstores
where o.id is null;

--4.Select All Main Categories and Their Subcategories. Select the name of the main categories (which have a NULL in the parent_id column) and the name of their direct subcategory (if one exists). Name the first column category and the second column subcategory.

select ca1.name as category,
       ca2.name as subcategory
from session_1_category ca1
left join session_1_category ca2 on ca1.id = ca2.parent_id
where ca2.name is not null