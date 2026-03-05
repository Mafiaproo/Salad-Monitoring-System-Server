SET GLOBAL event_scheduler = ON;
USE SMSDb;

CREATE TABLE clients( 
	uuid VARCHAR(40) PRIMARY KEY,
	client_name VARCHAR(30),
	internUuid VARCHAR(36),
	last_seen DATETIME,
	client_status VARCHAR(20)
);

CREATE TABLE users(
	id INT PRIMARY KEY,
	username VARCHAR(30),
	email VARCHAR(50),
	password_hash VARCHAR(50),
	salad_dashboard_key VARCHAR(50),
	salad_machine_key VARCHAR(50)
);


CREATE EVENT CLIENTS_DELETE_SCH
ON SCHEDULE
EVERY 5 MINUTE
DO DELETE FROM clients ;


