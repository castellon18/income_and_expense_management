CREATE DATABASE db_income_and_expense_management;
GO

USE db_income_and_expense_management;
GO

CREATE TABLE users(
	user_id INT IDENTITY(1,1) NOT NULL,
	user_account_name NVARCHAR(50) NOT NULL,
	normalized_user_account_name NVARCHAR(50) NOT NULL,
	email NVARCHAR(256) NOT NULL,
	normalized_email NVARCHAR(256) NOT NULL,
	password_hash NVARCHAR(MAX) NOT NULL,
	CONSTRAINT pk_user_id PRIMARY KEY(user_id),
	CONSTRAINT uq_user_account_name UNIQUE(user_account_name)
);
GO

CREATE TABLE account_types(
	account_type_id INT IDENTITY(1,1) NOT NULL,
	account_type_name NVARCHAR(50) NOT NULL,
	user_id INT NOT NULL,
	order_by INT NOT NULL,
	CONSTRAINT pk_account_type_id PRIMARY KEY(account_type_id),
	CONSTRAINT fk_account_types_users FOREIGN KEY(user_id) REFERENCES users(user_id)
);
GO

CREATE TABLE accounts(
	account_id INT IDENTITY(1,1) NOT NULL,
	account_name NVARCHAR(50) NOT NULL,
	account_type_id INT NOT NULL,
	account_balance DECIMAL(18,4) NOT NULL,
	account_description NVARCHAR(1000)
	CONSTRAINT pk_account_id PRIMARY KEY(account_id),
	CONSTRAINT fk_accounts_account_types FOREIGN KEY(account_type_id) REFERENCES account_types(account_type_id)
);
GO

CREATE TABLE operation_types(
	operation_type_id INT IDENTITY(1,1) NOT NULL,
	description_operation NVARCHAR(50) NOT NULL,
	CONSTRAINT pk_operation_type_id PRIMARY KEY(operation_type_id)
);
GO

CREATE TABLE categories(
	category_id INT IDENTITY(1,1) NOT NULL,
	category_name NVARCHAR(50) NOT NULL,
	operation_type_id INT NOT NULL,
	user_id INT NOT NULL,
	CONSTRAINT pk_category_id PRIMARY KEY(category_id),
	CONSTRAINT fk_categories_operation_types FOREIGN KEY(operation_type_id) REFERENCES operation_types(operation_type_id),
	CONSTRAINT fk_categories_users FOREIGN KEY(user_id) REFERENCES users(user_id)
);
GO

CREATE TABLE transactions(
	transaction_id INT IDENTITY(1,1) NOT NULL,
	user_id INT NOT NULL,
	transaction_date DATETIME NOT NULL,
	amount DECIMAL(18,4) NOT NULL,
	account_id INT NOT NULL,
	category_id INT NOT NULL,
	transaction_note NVARCHAR(1000),
	CONSTRAINT pk_transaction_id PRIMARY KEY(transaction_id),
	CONSTRAINT fk_transactions_accounts FOREIGN KEY(account_id) REFERENCES accounts(account_id),
	CONSTRAINT fk_transactions_users FOREIGN KEY(user_id) REFERENCES users(user_id),
	CONSTRAINT fk_transactions_categories FOREIGN KEY(category_id) REFERENCES categories(category_id)
);
GO

-- Insertar trasacciones

INSERT INTO dbo.transactions(
	user_id,
	transaction_date,
	amount,
	operation_type_id,
	transaction_note
) VALUES(
	'Felipe Gavilan',
	'2026-01-01',
	12500.66,
	1,
	'Nota de ejemplo'
),(
	'Federico Canton',
	'2026-01-20',
	13690.23,
	2,
	'Nota de ejemplo 2'
),(
	'Cristina Moltanban',
	'2026-02-15',
	4500.369,
	1,
	'Nota de ejemplo 3'
),(
	'Penelope Cruz',
	'2026-01-30',
	7865.1235,
	2,
	'Nota de ejemplo 4'
);
GO

-- Insertar un usuario
INSERT INTO dbo.users(
	user_account_name,
	email,
	normalized_email,
	password_hash
) VALUES(
		'cramirez',
		'cramirez@yahoo.com',
		'CRAMIREZ@YAHOO.COM',
		'123cramirez'
);
GO

SELECT * FROM account_types;
GO