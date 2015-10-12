BEGIN TRANSACTION;

INSERT INTO streaming_service
(streaming_service_id, name, url)
VALUES
(5, 'Viewster', 'https://www.viewster.com/'),
(6, 'Daisuki', 'http://www.daisuki.net/'),
(7, 'Netflix', 'https://www.netflix.com/');

COMMIT TRANSACTION;