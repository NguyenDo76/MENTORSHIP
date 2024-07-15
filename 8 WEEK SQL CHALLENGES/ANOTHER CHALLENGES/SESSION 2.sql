-- The runner table contains the following columns:
--      id stores the unique ID of the runner.
--      name stores the runner's name.
--      main_distance stores the distance (in meters) that the runner runs during events.
--      age stores the runner's age.
--      is_female indicates if the runner is male or female.

-- The event table contains the following columns:
--      id stores the unique ID of the event.
--      name stores the name of the event (e.g. London Marathon, Warsaw Runs, or New Year Run).
--      start_date stores the date of the event.
--      city stores the city where the event takes place.
	
-- The runner_event table contains the following columns:
--      runner_id stores the ID of the runner.
--      event_id stores the ID of the event.

--QUESTION

--1.Create tables

drop table if exists session_2_runner;
Create table session_2_runner (
		id int,
		name varchar (50),
		main_distance decimal (10,2),
		age int,
		is_female int
		);
insert into session_2_runner values
    ('1','Cheryl','10','25','1'),
    ('2','Jay','15.2','32','0'),
    ('3','Jaime','25.8','45','1'),
    ('4','Jackie','30.9','51','0'),
    ('5','Elijah','26.3','19','1');


drop table if exists session_2_event;
Create table session_2_event (
		id int,
		name varchar (100),
		start_date datetime,
		city varchar (50)
		);
insert into session_2_event values
    ('1','London Marathon','2023-01-15','New York'),
    ('2','Warsaw Runs','2023-03-15','New York'),
    ('3','New Year Run','2023-05-15','Chicago');


drop table if exists session_2_runner_event;
Create table session_2_runner_event (
		runner_id int,
		event_id int
		);
insert into session_2_runner_event values
    ('1','1'),
    ('2','2'),
    ('3','1'),
    ('4','3'),
    ('5','3'),
	('1','2'),
	('2','1'),
	('3','1'),
	('4','2'),
	('5','3'),
	('5','1'),
	('1','2'),
    ('1','3');


--2.Organize Runners Into Groups: Select the main distance and the number of runners that run the given distance (runners_number). Display only those rows where the number of runners is greater than 3.

select runner_id,main_distance,
	count (runner_id) as runners_number
from session_2_runner_event e
join session_2_runner r on e.runner_id = r.id
group by runner_id,main_distance
having count (runner_id) > 3;

--3.How Many Runners Participate in Each Event: Display the event name and the number of club members that take part in this event (call this column runner_count). Note that there may be events in which no club members participate. For these events, the runner_count should equal 0.

with cte_runner as 	(select e.name,
				count (runner_id) as runner_count
			from session_2_runner_event re
			right join session_2_event e on re.event_id = e.id
			group by e.name)

select 	name,
	case when runner_count is null then 0 else runner_count end as runner_count
from cte_runner;
--4.Group Runners by Main Distance and Age: Display the distance and the number of runners there are for the following age categories: under 20, 20_29, 30_39, 40_49, and over 50. Use the following column aliases: under_20, age_20_29, age_30_39, age_40_49, and over_50.

select main_distance,
sum (case when age < 20 then 1 else 0 end) as under_20,
sum (case when age between 20 and 29 then 1 else 0 end) as age_20_29,
sum (case when age between 30 and 39 then 1 else 0 end) as age_30_39,
sum (case when age between 40 and 49 then 1 else 0 end) as age_40_49,
sum (case when age > 50 then 1 else 0 end) as over_50
from session_2_runner
group by main_distance