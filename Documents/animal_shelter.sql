CREATE DATABASE animal_shelter;

CREATE TABLE shelters (
    shelter_id SERIAL PRIMARY KEY,
    shelter_name VARCHAR(100) NOT NULL,
    address VARCHAR(200),
    phone VARCHAR(20),
    email VARCHAR(100) UNIQUE
);

CREATE TABLE pets (
    pet_id SERIAL PRIMARY KEY,
    pet_name VARCHAR(100) NOT NULL,
    type VARCHAR(50) NOT NULL,
    age INT,
    gender VARCHAR(10),
    description TEXT,
    health TEXT,
    photoURL TEXT,
    status VARCHAR(20) DEFAULT 'available'
        CHECK (status IN ('available', 'adopted', 'treatment', 'reserved')),
    shelter_id INT REFERENCES shelters(shelter_id) ON DELETE CASCADE
);


CREATE TABLE users (
    user_id SERIAL PRIMARY KEY,
    user_name VARCHAR(100) NOT NULL,
    user_surname VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password VARCHAR(200) NOT NULL,
    phone VARCHAR(20),
    role VARCHAR(20) CHECK (role IN ('user', 'shelter_admin'))
);

CREATE TABLE adoption_requests (
    adopt_id SERIAL PRIMARY KEY,
    user_id INT REFERENCES users(user_id) ON DELETE CASCADE,
    pet_id INT REFERENCES pets(pet_id) ON DELETE CASCADE,
    date DATE DEFAULT CURRENT_DATE,
    status VARCHAR(20) CHECK (status IN ('new', 'in_progress', 'approved', 'rejected'))

);
