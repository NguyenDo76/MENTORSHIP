Drop table if exists News
Create table News ( News_ID int,
                    Title text,
                    Description text,
                    Link nvarchar (50),
                    Guid nvarchar (50),
                    PubDate datetime,
                    UpdatedDate datetime,                      
                    ImageURL nvarchar (50),
                    SourceID int,
                    CategoryID int                      
                  )

Drop table if exists [User]
Create table [User] ( UserID int,
                      UserName nvarchar (100),
                      UserPassword nvarchar (100),
                      Email nvarchar (100),
                      JoinedDate datetime,
                      LastedSignOut datetime                 
                    )

Drop table if exists Source
Create table Source ( SourceID int,
                      SourceName nvarchar (50),
                      URL nvarchar (100),                                  
                    )
Insert into Source values 
(1, 'Tuoi tre', 'https://tuoitre.vn/rss.htm'),
(2, 'Thanh nien', 'https://thanhnien.vn/rss.html'),
(3, 'Vietnamexpress', 'https://vnexpress.net/rss')

Drop table if exists Categories
Create table Categories ( CategoryID int,
                          CategoryName nvarchar (50),                                                      
                        )


Drop table if exists Tags
Create table Tags ( TagID int,
                    TagName nvarchar (100),                                                      
                  )

Drop table if exists Post
Create table Post ( PostID int,
                    UserID int,
                    NewsID int,
                    CategoryID int,
                    [View] int,
                    [Like] int,
                    Comment int,
                    Bookmark int                                                    
                  )                  

Drop table if exists UserCategory
Create table UserCategory ( UserCategoryID int,
                            UserID int,
                            CategoryFlavorID int,
                            CreatedDate datetime
                          )

Drop table if exists UserTags
Create table UserTags ( UserTagID int,
                        UserID int,
                        TagFlavorID int,
                        CreatedDate datetime
                          )
                          
Drop table if exists NewsTags
Create table NewsTags ( NewsTagID int,
                        News_ID int,
                        TagID int,
                        CreatedDate datetime
                          )  

