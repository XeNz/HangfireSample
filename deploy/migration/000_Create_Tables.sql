 SET search_path TO public;

 create table public."CommentReadHistories"
 (
   "Id"        serial,
   "CommentId" int,
   "ReadDate"  date
 );

 create unique index """CommentReadHistories""_""Id""_uindex"
   on "CommentReadHistories" ("Id");

 alter table "CommentReadHistories"
   add constraint """CommentReadHistories""_pk"
     primary key ("Id");

