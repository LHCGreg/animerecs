BEGIN TRANSACTION;

INSERT INTO streaming_service
(streaming_service_id, name, url)
VALUES
(8, 'Anime Network', 'http://www.theanimenetwork.com/');

COMMIT TRANSACTION;