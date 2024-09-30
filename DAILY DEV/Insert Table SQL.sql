Drop table if exists News
Create table News ( NewsID int,
                    Title text,
                    Description text,
                    PublicationDate datetime,
                    UpdatedDate datetime,
                    RSS_ID int,
                    Content text,
                    ImageURL varchar (50),
                    ViewID int,
                    LikeID int,
                    CommentID int
                  )
Insert into News values
(1,'Filiberto','Cormier','2024-09-01T23:28:06.160','2024-09-01T23:28:06.160',1,'Content','https://loremflickr.com/640/480',1,1,1),
(2,'Ollie','Erdman','2024-09-01T23:29:06.160','2024-09-01T23:29:06.160',2,'Content','https://loremflickr.com/640/480',2,2,2),
(3,'Kylee','Spinka','2024-09-01T23:30:06.160','2024-09-01T23:30:06.160',3,'Content','https://loremflickr.com/640/480',3,3,3),
(4,'Cleveland','Lockman','2024-09-01T23:31:06.160','2024-09-01T23:31:06.160',4,'Content','https://loremflickr.com/640/480',4,4,4),
(5,'Krystal','Metz','2024-09-01T23:32:06.160','2024-09-01T23:32:06.160',5,'Content','https://loremflickr.com/640/480',5,5,5),
(6,'Jacques','OKon','2024-09-01T23:33:06.160','2024-09-01T23:33:06.160',6,'Content','https://loremflickr.com/640/480',6,6,6),
(7,'Joanny','Feeney','2024-09-01T23:34:06.160','2024-09-01T23:34:06.160',7,'Content','https://loremflickr.com/640/480',7,7,7),
(8,'Abner','Mann','2024-09-01T23:35:06.160','2024-09-01T23:35:06.160',8,'Content','https://loremflickr.com/640/480',8,8,8),
(9,'Ernestina','Hirthe','2024-09-01T23:36:06.160','2024-09-01T23:36:06.160',9,'Content','https://loremflickr.com/640/480',9,9,9),
(10,'Tessie','Stehr','2024-09-01T23:37:06.160','2024-09-01T23:37:06.160',10,'Content','https://loremflickr.com/640/480',10,10,10);

Drop table if exists RSS             
Create table RSS (  RSS_ID int,
                    URL nvarchar (50),
                    SourceID int,
                    TagID int
                 )
Insert into RSS values
(1,'https://tuoitre.vn/rss/the-gioi.rss',1,1),
(2,'https://thanhnien.vn/rss/kinh-te.rss',2,2),
(3,'https://vnexpress.net/rss/gia-dinh.rss',3,3),
(4,'https://kenh14.vn/musik.rss',4,4),
(5,'https://tuoitre.vn/rss/phap-luat.rss',1,5),
(6,'https://thanhnien.vn/rss/giao-duc.rss',2,6),
(7,'https://vnexpress.net/rss/the-thao.rss',3,1),
(8,'https://kenh14.vn/doi-song.rss',4,3),
(9,'https://vnexpress.net/rss/phap-luat.rss',3,5),
(10,'https://vnexpress.net/rss/giao-duc.rss',3,6);

Drop table if exists Source          
Create table Source (SourceID int,
                     SourceName nvarchar(50),
                    )

Insert into Source values
(1,'Tuoitre'),
(2,'Thanhnien'),
(3,'Vietnamexpress'),
(4,'Kenh14');

Drop table if exists Tags           
Create table Tags ( TagID int,
                    TagName nvarchar (50),
                  )
Insert into Tags values
(1,'Thethao'),
(2,'Kinhte'),
(3,'Doisong'),
(4,'Amnhac'),
(5,'Phapluat'),
(6,'Giaoduc');

Drop table if exists Comment      
Create table Comment (CommentID int,
                      UserID int,
                      CreatedDate datetime,
                      Content text
                     )
Insert into Comment values
(1,1,'2024-09-01T23:28:06.160', 'great'),
(2,2,'2024-09-01T23:29:06.160', 'good job'),
(3,3,'2024-09-01T23:30:06.160', 'so cool'),
(4,4,'2024-09-01T23:31:06.160', 'great'),
(5,5,'2024-09-01T23:32:06.160', 'great'),
(6,6,'2024-09-01T23:33:06.160', 'great'),
(7,7,'2024-09-01T23:34:06.160', 'amazing!!!'),
(8,8,'2024-09-01T23:35:06.160', 'great'),
(9,9,'2024-09-01T23:36:06.160', 'great'),
(10,10,'2024-09-01T23:37:06.160', 'good job');

Drop table if exists [Like]
Create table [Like] (LikeID int,
                     UserID int,
                     CreatedDate datetime
                    )
Insert into [Like] values
(1,1,'2024-09-01T23:28:06.160'),
(2,2,'2024-09-01T23:29:06.160'),
(3,3,'2024-09-01T23:30:06.160'),
(4,4,'2024-09-01T23:31:06.160'),
(5,5,'2024-09-01T23:32:06.160'),
(6,6,'2024-09-01T23:33:06.160'),
(7,7,'2024-09-01T23:34:06.160'),
(8,8,'2024-09-01T23:35:06.160'),
(9,9,'2024-09-01T23:36:06.160'),
(10,10,'2024-09-01T23:37:06.160');

Drop table if exists [View]
Create table [View] (ViewID int,
                     UserID int,
                     CreatedDate datetime
                    )
Insert into [View] values                    
(1,1,'2024-09-01T23:28:06.160'),
(2,2,'2024-09-01T23:29:06.160'),
(3,3,'2024-09-01T23:30:06.160'),
(4,4,'2024-09-01T23:31:06.160'),
(5,5,'2024-09-01T23:32:06.160'),
(6,6,'2024-09-01T23:33:06.160'),
(7,7,'2024-09-01T23:34:06.160'),
(8,8,'2024-09-01T23:35:06.160'),
(9,9,'2024-09-01T23:36:06.160'),
(10,10,'2024-09-01T23:37:06.160');


Drop table if exists Bookmarks
Create table Bookmarks (BookmarkID int,
                       UserID int,
                       CreatedDate datetime
                      )
Insert into [Bookmarks] values                    
(1,1,'2024-09-01T23:28:06.160'),
(2,2,'2024-09-01T23:29:06.160'),
(3,3,'2024-09-01T23:30:06.160'),
(4,4,'2024-09-01T23:31:06.160'),
(5,5,'2024-09-01T23:32:06.160'),
(6,6,'2024-09-01T23:33:06.160'),
(7,7,'2024-09-01T23:34:06.160'),
(8,8,'2024-09-01T23:35:06.160'),
(9,9,'2024-09-01T23:36:06.160'),
(10,10,'2024-09-01T23:37:06.160');
                    
Drop table if exists [User]
Create table [User] (UserID int,
                     UserName nvarchar (100),
                     UserPassword nvarchar (100),
                     Email nvarchar(100),
                     JoinedDate datetime,
                     LastedSignOut datetime,
                    )
Insert into [User] values
(1,'Rossie_Wyman69','VRdfX0T8P4inbwG','Summer_Koelpin20@gmail.com','2024-08-01T14:03:20.819','2024-09-01T14:03:20.819'),
(2,'Myrtice_Kuvalis','nae9PlTPynNfo_M','Abner1@hotmail.com','2024-08-01T14:04:20.819','2024-09-01T14:04:20.819'),
(3,'Enid_Bergnaum','DYkCIiFjxBmsl_t','Coy74@yahoo.com','2024-08-01T14:05:20.819','2024-09-01T14:05:20.819'),
(4,'Chris_Friesen16','4kGY12yu7QyCuKR','Zella_Shanahan95@hotmail.com','2024-08-01T14:06:20.819','2024-09-01T14:06:20.819'),
(5,'Turner.Braun5','TuVthM3xo17shHJ','Johan_Jenkins@yahoo.com','2024-08-01T14:07:20.819','2024-09-01T14:07:20.819'),
(6,'Cortney.Hackett','EgxmTBfsRU50nJV','Christiana.Towne@gmail.com','2024-08-01T14:08:20.819','2024-09-01T14:08:20.819'),
(7,'Jared.Johnson99','xtc6xlLpAHCA5Bv','Dixie28@yahoo.com','2024-08-01T14:09:20.819','2024-09-01T14:09:20.819'),
(8,'Rollin.Treutel15','Ja_KAQxD9HkPrPo','Devyn_Borer@hotmail.com','2024-08-01T14:10:20.819','2024-09-01T14:10:20.819'),
(9,'Dina64','Xq_NYA_lv9AM7UB','Maiya.Murazik@gmail.com','2024-08-01T14:11:20.819','2024-09-01T14:11:20.819'),
(10,'Georgette64','CSziRO4qjJl1Sr9','Columbus59@hotmail.com','2024-08-01T14:12:20.819','2024-09-01T14:12:20.819')



