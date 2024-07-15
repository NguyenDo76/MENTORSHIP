-- The customers table has 15 columns:
--      customer_id stores the ID of the customer.
--      email stores the customer's email address.
--      full_name stores the customer's full name.
--      address stores the customer's street and house number.
--      city stores the city where the customer lives.
--      region stores the customer's region (not always applicable).
--      postal_code stores the customer's ZIP/post code.
--      country stores the customer's country.
--      phone stores the customer's phone number.
--      registration_date stores the date on which the customer registered.
--      channel_id stores the ID of the channel through which the customer found the shop.
--      first_order_id stores the ID of the first order made by the customer.
--      first_order_date stores the date of the customer's first order.
--      last_order_id stores the ID of the customer's last (i.e. most recent) order.
--      last_order_date stores the date of the customer's last order.

-- The orders table has the following columns:
--      order_id stores the ID of the order.
--      customer_id stores the ID of the customer who placed the order.
--      order_date stores the date when the order was placed.
--      total_amount stores the total amount paid for the order.
--      ship_name stores the name of the person to whom the order was sent.
--      ship_address stores the address (house number and street) where the order was sent.
--      ship_city stores the city where the order was sent.
--      ship_region stores the region in which the city is located.
--      ship_postalcode stores the destination post code.
--      ship_country stores the destination country.
--      shipped_date stores the date when the order was shipped.
	
-- The products table has the following columns:
--      product_id stores the ID of the product.
--      product_name stores the name of the product.
--      category_id stores the category to which the product belongs.
--      unit_price stores the price for one unit of the product (e.g. per bottle, pack, etc.).
--      discontinued indicates if the product is no longer sold.

-- The categories table has the following columns:
--      category_id stores the ID of the category.
--      category_name stores the name of the category.
--      description stores a short description of the category.

-- The order_items table has the following columns:
--      order_id stores the ID of the order in which the product was bought.
--      product_id stores the ID of the product purchased in the order.
--      unit_price stores the per-unit price of the product. (Note that this can be different from the price in the product's category; the price can change over time and discounts can be applied.)
--      quantity stores the number of units bought in the order.
--      discount stores the discount applied to the given product.
	
-- The channels table has the following columns:
--      id stores the ID of the channel.
--      channel_name stores the name of the channel through which the customer found the shop.

--QUESTION

--1.Create table
drop table if exists session_3_customers;
Create table session_3_customers (
		customer_id int,
		email varchar (100),
		full_name varchar (100),
		address varchar (max),
		city varchar (50),
		region varchar (50),
		postal_code varchar (50),
		country varchar (50),
		phone varchar (50),
		registration_date datetime,
		channel_id int,
		first_order_id int,
		first_order_date datetime,
		last_order_id int,
		last_order_date datetime
		);
insert into session_3_customers values
    ('1', 'Mona.Powlowski28@gmail.com', 'Ricardo OKon', '09609 Rachel Wall', 'Fort Kelli', 'Borders', '52696-4068', 'Faroe Islands', '430-299-5182 x38257', '2023-07-01T11:34:48.235', '1', '1', '2023-07-10T11:34:48.235', '1', '2023-08-01T09:09:00.000'),
    ('2', 'Jaren_Hermann@yahoo.com', 'Felicia Hilll', '767 Kale Oval', 'Sierra Vista', 'Berkshire', '34840', 'Norfolk Island', '364-483-7337 x5540', '2023-07-02T04:14:48.235', '2', '2', '2023-07-11T04:14:48.235', '2', '2023-08-02T01:49:00.000'),
    ('3', 'Shannon.Ryan@yahoo.com', 'Jody Marquardt', '1184 Gerlach Common', 'Mohrfield', 'Berkshire', '93668-2988', 'Vietnam', '704-792-0417 x6683', '2023-07-02T20:54:48.235', '2', '3', '2023-07-11T20:54:48.235', '3', '2023-08-02T18:29:00.000'),
    ('4', 'Marianne.Mann88@yahoo.com', 'Irving Oberbrunner', '77443 Metz Stravenue', 'Wilson', 'Berkshire', '02568', 'Faroe Islands', '1-532-248-2871 x26522', '2023-07-03T13:34:48.235', '2', '4', '2023-07-12T13:34:48.235', '4', '2023-08-03T11:09:00.000'),
    ('5', 'Hallie_Mosciski79@gmail.com', 'Clayton Gutkowski', '94662 Eleonore Corner', 'Bashirianside', 'Buckinghamshire', '27594-6695', 'China', '1-780-719-8583 x876', '2023-07-04T06:14:48.235', '1', '5', '2023-07-13T06:14:48.235', '5', '2023-08-04T03:49:00.000'),
    ('6', 'Cleveland41@hotmail.com', 'Vernon Bogan', '774 Golden Knolls', 'New Devin', 'Cambridgeshire', '52609', 'Papua New Guinea', '391.543.2764 x81630', '2023-07-04T22:54:48.235', '1', '6', '2023-07-13T22:54:48.235', '6', '2023-08-04T20:29:00.000'),
    ('7', 'Ole_OReilly@yahoo.com', 'Lance Hettinger', '3575 Schumm Motorway', 'Lazarostad', 'Bedfordshire', '71472-0024', 'Norfolk Island', '700.630.4168 x4887', '2023-07-05T15:34:48.235', '2', '7', '2023-07-14T15:34:48.235', '7', '2023-08-05T13:09:00.000'),
    ('8', 'Holly.Langworth@gmail.com', 'Alison McClure', '5645 Jerde Junctions', 'Port Anahiport', 'Berkshire', '75684', 'Faroe Islands', '1-229-934-8819 x431', '2023-07-06T08:14:48.235', '2', '8', '2023-07-15T08:14:48.235', '8', '2023-08-06T05:49:00.000'),
    ('9', 'June.Greenholt@yahoo.com', 'Hazel Ebert', '69857 Kassulke River', 'Kettering', 'Berkshire', '87834-8068', 'Tokelau', '(252) 890-5382 x37241', '2023-07-07T00:54:48.235', '2', '9', '2023-07-16T00:54:48.235', '9', '2023-08-06T22:29:00.000'),
    ('10', 'Geovanni14@hotmail.com', 'Kristopher Adams', '6227 Bechtelar Pass', 'Ziemehaven', 'Cambridgeshire', '13550-0453', 'French Polynesia', '971-780-6326 x9159', '2023-07-07T17:34:48.235', '2', '10', '2023-07-16T17:34:48.235', '10', '2023-08-07T15:09:00.000');


drop table if exists session_3_orders;
Create table session_3_orders (
        order_id int,
	    customer_id int,
	    order_date datetime,
	    total_amount int,
    	ship_name varchar (100),
	    ship_address varchar (100),
	    ship_city varchar (100),
	    ship_region varchar (100),
	    ship_postalcode varchar (100),
	    ship_country varchar (100),
	    shipped_date datetime
        );
insert into session_3_orders values
    ('1', '1', '2023-07-10T11:34:48.235', '11', 'Dr. Myra Sawayn', '421 Rogahn Glens', 'Tuscaloosa', 'Avon', '69519', 'Zimbabwe', '2023-07-12T11:34:48.235'),
    ('2', '2', '2023-07-11T04:14:48.235', '81', 'Rebecca Hilll', '974 Jody Coves', 'Mission Viejo', 'Cambridgeshire', '34078', 'Bahamas', '2023-07-13T04:14:48.235'),
    ('3', '3', '2023-07-11T20:54:48.235', '9', 'Jean Gusikowski Jr.', '2967 Braden Burgs', 'Pittsburgh', 'Bedfordshire', '86639-6040', 'French Guiana', '2023-07-13T20:54:48.235'),
    ('4', '4', '2023-07-12T13:34:48.235', '67', 'Ms. Kenny Hermann', '00517 Turcotte Extensions', 'Salinas', 'Borders', '65019', 'Portugal', '2023-07-14T13:34:48.235'),
    ('5', '5', '2023-07-13T06:14:48.235', '66', 'Tamara Hand Sr.', '8015 Lurline Overpass', 'Wylie', 'Buckinghamshire', '27236-3129', 'Tunisia', '2023-07-15T06:14:48.235'),
    ('6', '6', '2023-07-13T22:54:48.235', '9', 'Dan Rolfson', '944 Kali Manor', 'Olathe', 'Buckinghamshire', '99754-5087', 'Iran', '2023-07-15T22:54:48.235'),
    ('7', '7', '2023-07-14T15:34:48.235', '44', 'Emilio Wisozk', '1393 Delphine Mews', 'San Marcos', 'Avon', '22063-7058', 'Cocos (Keeling) Islands', '2023-07-16T15:34:48.235'),
    ('8', '8', '2023-07-15T08:14:48.235', '26', 'Rodney Smitham', '26129 Esmeralda Brooks', 'Lake Havasu City', 'Borders', '41671', 'Belize', '2023-07-17T08:14:48.235'),
    ('9', '9', '2023-07-16T00:54:48.235', '36', 'Jose Fisher', '21502 Grady Trace', 'Jonesboro', 'Borders', '72922', 'Iran', '2023-07-18T00:54:48.235'),
    ('10', '10', '2023-07-16T17:34:48.235', '49', 'Perry Stroman', '6744 Brakus Ford', 'Salem', 'Borders', '49422', 'Thailand', '2023-07-18T17:34:48.235'),
    ('11', '3', '2023-07-17T10:14:48.235', '51', 'Ray Kuvalis PhD', '3132 Hudson Parks', 'Longmont', 'Cambridgeshire', '21478', 'New Zealand', '2023-07-19T10:14:48.235'),
    ('12', '5', '2023-07-18T02:54:48.235', '9', 'Benny Kiehn', '041 Fannie Lane', 'Rochester Hills', 'Bedfordshire', '15712', 'Bouvet Island (Bouvetoya)', '2023-07-20T02:54:48.235'),
    ('13', '6', '2023-07-18T19:34:48.235', '21', 'Luther Jacobs III', '8230 Mills Pike', 'Surprise', 'Borders', '21073', 'Swaziland', '2023-07-20T19:34:48.235'),
    ('14', '4', '2023-07-19T12:14:48.235', '17', 'Roderick Davis', '9843 Rippin Loop', 'Rosemead', 'Berkshire', '51243-3054', 'Honduras', '2023-07-21T12:14:48.235'),
    ('15', '2', '2023-07-20T04:54:48.235', '93', 'Isaac Hermann', '6476 Wiza Club', 'Frederick', 'Borders', '55845', 'Saint Barthelemy', '2023-07-22T04:54:48.235'),
    ('16', '1', '2023-07-20T21:34:48.235', '31', 'Lowell Greenfelder PhD', '477 West Islands', 'La Crosse', 'Cambridgeshire', '25058', 'Malta', '2023-07-22T21:34:48.235'),
    ('17', '9', '2023-07-21T14:14:48.235', '43', 'Bennie Thompson', '837 Schmeler Fort', 'Huntington Beach', 'Buckinghamshire', '33940-3346', 'Samoa', '2023-07-23T14:14:48.235'),
    ('18', '10', '2023-07-22T06:54:48.235', '72', 'Jennie Hoppe', '47184 Blanda Mill', 'Federal Way', 'Buckinghamshire', '94772', 'Somalia', '2023-07-24T06:54:48.235'),
    ('19', '7', '2023-07-22T23:34:48.235', '77', 'Emmett Collins', '59981 Devon Stream', 'La Mesa', 'Cambridgeshire', '84202-6839', 'Namibia', '2023-07-24T23:34:48.235'),
    ('20', '6', '2023-07-23T16:14:48.235', '72', 'Ms. Wesley Bernhard', '84142 Olson Keys', 'Warner Robins', 'Berkshire', '22278', 'Lebanon', '2023-07-25T16:14:48.235'),
    ('21', '7', '2023-07-24T08:54:48.235', '54', 'Amy Gislason', '0708 Dereck Forks', 'Morgan Hill', 'Berkshire', '78112', 'Nigeria', '2023-07-26T08:54:48.235'),
    ('22', '9', '2023-07-25T01:34:48.235', '19', 'Darla Okuneva', '4933 Louie Row', 'Richmond', 'Cambridgeshire', '66384-3544', 'Virgin Islands, U.S.', '2023-07-27T01:34:48.235'),
    ('23', '5', '2023-07-25T18:14:48.235', '15', 'Jimmie Walsh', '60652 Jorge Mountains', 'St. George', 'Bedfordshire', '09098-6760', 'Bolivia', '2023-07-27T18:14:48.235'),
    ('24', '9', '2023-07-26T10:54:48.235', '76', 'Mr. Earnest Heathcote', '13889 Raynor Place', 'Brentwood', 'Bedfordshire', '25521', 'Guinea', '2023-07-28T10:54:48.235'),
    ('25', '10', '2023-07-27T03:34:48.235', '69', 'Frank Marks', '708 Deon Branch', 'South Bend', 'Berkshire', '52663-3659', 'Thailand', '2023-07-29T03:34:48.235'),
    ('26', '8', '2023-07-27T20:14:48.235', '33', 'Vera Klocko', '00846 Lesch Walk', 'Poinciana', 'Avon', '68134', 'Yemen', '2023-07-29T20:14:48.235'),
    ('27', '5', '2023-07-28T12:54:48.235', '58', 'Roman Kiehn', '62888 Sipes Estates', 'Tigard', 'Cambridgeshire', '22826', 'Cote dIvoire', '2023-07-30T12:54:48.235');


drop table if exists session_3_products;
Create table session_3_products (
        product_id int,
		product_name varchar (100),
		category_id int,
		unit_price decimal (10,2),
		discontinued varchar (10)
        );
insert into session_3_products values
    ('1', 'Handcrafted Cotton Soap', '1', '15.00','N'),
    ('2', 'Modern Soft Chips', '2', '6.00','N'),
    ('3', 'Handcrafted Wooden Fish', '3', '4.00','N'),
    ('4', 'Generic Cotton Chair', '3', '32.00','N'),
    ('5', 'Sleek Concrete Tuna', '4', '20.00','N'),
    ('6', 'Small Granite Gloves', '5', '38.00','N'),
    ('7', 'Handcrafted Frozen Keyboard', '3', '4.00','Y'),
    ('8', 'Luxurious Concrete Cheese', '2', '26.00','N'),
    ('9', 'Handcrafted Rubber Bacon', '1', '20.00','Y'),
    ('10', 'Rustic Metal Sausages', '4', '13.00','N'),
    ('11', 'Practical Rubber Chips', '2', '7.00','N'),
    ('12', 'Awesome Metal Mouse', '5', '12.00','N'),
    ('13', 'Intelligent Metal Pizza', '5', '45.00','Y'),
    ('14', 'Bespoke Frozen Pizza', '2', '36.00','Y'),
    ('15', 'Fantastic Concrete Tuna', '4', '36.00','N'),
    ('16', 'Recycled Bronze Gloves', '3', '6.00','Y'),
    ('17', 'Rustic Soft Chips', '4', '6.00','N'),
    ('18', 'Handmade Metal Pizza', '1', '6.00','N'),
    ('19', 'Fantastic Rubber Chicken', '2', '40.00','Y'),
    ('20', 'Handmade Rubber Shirt', '5', '7.00','Y');


drop table if exists session_3_categories;
Create table session_3_categories (
        category_id int,
		category_name varchar (100),
		description text,
        );
insert into session_3_categories values
    ('1','Soft','abcdef fnsdufnds'),
    ('2','Fresh','fdjkfbdkf chsdfhsdo'),
    ('3','Granite','fbsdkfcd bdshkbs'),
    ('4','Concrete','dkcfbsd fbsdbfcsdk'),
    ('5','Metal','dfbsdkb fndufhsdiy');


drop table if exists session_3_order_items;
Create table session_3_order_items (
        order_id int,
		product_id int,
		unit_price decimal (10,2),
		quantity int,
		discount decimal (10,2)
        );
insert into session_3_order_items values
    ('1','12','20','11','0'),
    ('2','10','80','50','2'),
    ('2','18','30','31','1'),
    ('3','20','26','9','0'),
    ('4','5','33','30','1'),
    ('4','7','11','37','1'),
    ('5','16','62','20','0'),
    ('5','9','58','41','1.5'),
    ('5','10','47','5','0'),
    ('6','16','68','9','0'),
    ('7','17','29','44','1.5'),
    ('8','18','65','26','0'),
    ('9','8','38','10','0'),
    ('9','3','28','26','0'),
    ('10','1','49','5','0'),
    ('10','2','63','4','0'),
    ('10','7','77','15','0'),
    ('10','13','68','15','0'),
    ('10','17','59','10','0'),
    ('11','17','60','40','1.5'),
    ('11','17','86','11','0'),
    ('12','17','45','9','0'),
    ('13','17','26','21','0'),
    ('14','17','33','17','0'),
    ('15','17','69','50','2'),
    ('15','17','85','43','1.5'),
    ('16','17','96','31','1'),
    ('17','17','24','43','1.5'),
    ('18','17','59','60','2.5'),
    ('18','17','63','12','0'),
    ('19','17','49','40','1.5'),
    ('19','17','16','37','1'),
    ('20','17','47','50','2'),
    ('20','17','35','10','0'),
    ('20','17','17','12','0'),
    ('21','17','20','54','2'),
    ('22','17','30','19','0'),
    ('23','17','89','15','0'),
    ('24','17','82','15','0'),
    ('24','17','86','15','0'),
    ('24','17','36','46','1.5'),
    ('25','17','79','45','1.5'),
    ('25','17','48','24','0'),
    ('26','17','39','33','1'),
    ('27','17','18','58','2');


drop table if exists session_3_channels;
Create table session_3_channels (
        id int,
		channel_name varchar (10)
        );
insert into session_3_channels values
    ('1','online'),
    ('2','store');


--2.List the Top 3 Most Expensive Orders

select top 3 order_id, (quantity * unit_price - quantity * discount) as revenue
from session_3_order_items
order by (quantity * unit_price - quantity * discount) desc

--3.Compute Deltas Between Consecutive Orders: In this exercise, we're going to compute the difference between two consecutive orders from the same customer. Show the ID of the order (order_id), the ID of the customer (customer_id), the total_amount of the order, the total_amount of the previous order based on the order_date (name the column previous_value), and the difference between the total_amount of the current order and the previous order (name the column delta).

select i.order_id, o.customer_id, total_amount,
       coalesce (lag (total_amount) over (partition by o.customer_id order by order_date asc),0) as previous_value,
       coalesce (total_amount - lag (total_amount) over (partition by o.customer_id order by order_date asc),0) as delta
from session_3_order_items i
join session_3_orders o on o.order_id = i.order_id
join session_3_customers c on c.customer_id = o.customer_id

--4. Compute the Running Total of Purchases per Customer: For each customer and their orders, show the following:
-- 	customer_id: the ID of the customer.
-- 	full_name: the full name of the customer.
-- 	order_id: the ID of the order.
-- 	order_date: the date of the order.
-- 	total_amount: the total spent on this order.
-- 	running_total: the running total spent by the given customer.

select o.customer_id, full_name, o.order_id, order_date, total_amount,
       sum (quantity * unit_price - quantity * discount) as running_total
from session_3_orders o
join session_3_customers c on c.customer_id = o.customer_id
join session_3_order_items i on i.order_id = o.order_id
group by o.customer_id, full_name, o.order_id, order_date, total_amount
