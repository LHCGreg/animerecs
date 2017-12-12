BEGIN TRANSACTION;

INSERT INTO streaming_service
(streaming_service_id, name, url)
VALUES
(11, 'Hidive', 'https://www.hidive.com');

COMMIT TRANSACTION;