Drop table if exists News
Create table News ( NewsID int,
                    Title text,
                    Description text,
                    CreatedDate datetime,
                    UpdatedDate datetime,
                    RSS_ID int,
                    ImageURL text,
                    ViewID int,
                    LikeID int,
                    CommentID int
                  );

Drop table if exists RSS             
Create table RSS (  RSS_ID int,
                    URL text,
                    SourceID int,
                    TagID int
                 );

Drop table if exists Source          
Create table Source (SourceID int,
                     SourceName text,
                     FollowID int
                    );

Drop table if exists Follow      
Create table Follow (FollowID int,
                     UserID int,
                     CreatedDate datetime
                    );

Drop table if exists Tags           
Create table Tags ( TagID int,
                    TagName text,
                  );

Drop table if exists Comment      
Create table Comment (CommentID int,
                      UserID int,
                      CreatedDate datetime
                     );

Drop table if exists [Like]
Create table [Like] (LikeID int,
                     UserID int,
                     CreatedDate datetime
                    );

Drop table if exists [View]
Create table [View] (ViewID int,
                     UserID int,
                     CreatedDate datetime
                    );
                    
Drop table if exists [User]
Create table [User] (UserID int,
                     UserName text,
                     UserPassword text,
                     JoinedDate datetime,
                     LastedSignOut datetime,
                     TagsID int
                    );

