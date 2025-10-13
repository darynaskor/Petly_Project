using System;
using Npgsql;

class Program
{
    static void Main()
    {
        // Рядок підключення до PostgreSQL
        string connString = "Host=192.168.1.103;Port=5432;Database=animal_shelter;Username=postgres;Password=newpassword";

        using var conn = new NpgsqlConnection(connString);
        conn.Open();

        Console.WriteLine(" Підключення до PostgreSQL успішне!");

        // Генерація тестових даних
        InsertShelters(conn, 4);   // додаємо 4 притулки
        InsertUsers(conn, 40);     // додаємо 40 користувачів
        InsertPets(conn, 50);      // додаємо 50 тварин
        InsertAdoptions(conn, 35); // додаємо 35 заявок на усиновлення

        Console.WriteLine("\n=== Вивід даних із таблиць ===");
        ShowTableSummary(conn, "shelters", "shelter_id, shelter_name, address, phone, email");
        ShowTableSummary(conn, "users", "user_id, user_name, user_surname, email, password, phone, role");
        ShowTableSummary(conn, "pets", "pet_id, pet_name, type, age, gender, description, health, photourl, shelter_id");
        ShowTableSummary(conn, "adoption_requests", "adopt_id, user_id, pet_id, date, status");

        Console.WriteLine("\n=== Вивід з JOIN ===");
        ShowPetsWithShelters(conn);   // вивід тварин разом із притулками
        ShowAdoptions(conn);          // вивід заявок із користувачами і тваринами

        Console.WriteLine("\n Все виконано успішно!");
    }

    // ----------------- Вставка притулків -----------------
    static void InsertShelters(NpgsqlConnection conn, int target)
    {
        // очищаємо таблицю перед вставкою
        using var clear = new NpgsqlCommand("TRUNCATE TABLE shelters RESTART IDENTITY CASCADE", conn);
        clear.ExecuteNonQuery();

        // додаємо задану кількість притулків
        for (int i = 1; i <= target; i++)
        {
            string phone = $"0{(100000000 + i):D9}"; // генеруємо телефон
            string sql = $"INSERT INTO shelters (shelter_name, address, phone, email) " +
                         $"VALUES ('Shelter {i}', 'Street {i}, City', '{phone}', 'shelter{i}@mail.com')";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }

    // ----------------- Вставка користувачів -----------------
    static void InsertUsers(NpgsqlConnection conn, int target)
    {
        // очищаємо таблицю
        using var clear = new NpgsqlCommand("TRUNCATE TABLE users RESTART IDENTITY CASCADE", conn);
        clear.ExecuteNonQuery();

        // додаємо користувачів
        for (int i = 1; i <= target; i++)
        {
            string phone = $"0{(200000000 + i):D9}"; // телефон
            string sql = $"INSERT INTO users (user_name, user_surname, email, password, phone, role) " +
                         $"VALUES ('User{i}', 'Surname{i}', 'user{i}@mail.com', 'pass{i}', '{phone}', 'user')";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }

    // ----------------- Вставка тварин -----------------
    static void InsertPets(NpgsqlConnection conn, int target)
    {
        // очищаємо таблицю
        using var clear = new NpgsqlCommand("TRUNCATE TABLE pets RESTART IDENTITY CASCADE", conn);
        clear.ExecuteNonQuery();

        string[] types = { "Cat", "Dog", "Rabbit" };  // види тварин
        string[] genders = { "Male", "Female" };      // стать

        // додаємо тварин
        for (int i = 1; i <= target; i++)
        {
            string type = types[(i - 1) % types.Length];
            string gender = genders[(i - 1) % genders.Length];

            // прив’язуємо тварину до притулку (рівномірно)
            int shelterId = ((i - 1) / (target / 4)) + 1;
            if (shelterId > 4) shelterId = 4;

            string sql = $"INSERT INTO pets (pet_name, type, age, gender, description, health, photourl, shelter_id) " +
                         $"VALUES ('Pet{i}', '{type}', {1 + i % 10}, '{gender}', 'Description {i}', 'Healthy', 'http://photo{i}.jpg', {shelterId})";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }

    // ----------------- Вставка заявок -----------------
    static void InsertAdoptions(NpgsqlConnection conn, int target)
    {
        // очищаємо таблицю
        using var clear = new NpgsqlCommand("TRUNCATE TABLE adoption_requests RESTART IDENTITY CASCADE", conn);
        clear.ExecuteNonQuery();

        string[] statuses = { "new", "in_progress", "approved", "rejected" }; // статуси

        // додаємо заявки
        for (int i = 1; i <= target; i++)
        {
            string status = statuses[(i - 1) % statuses.Length];
            int userId = ((i - 1) % 40) + 1;
            int petId = ((i - 1) % 50) + 1;

            string sql = $"INSERT INTO adoption_requests (user_id, pet_id, date, status) " +
                         $"VALUES ({userId}, {petId}, CURRENT_DATE - INTERVAL '{i} day', '{status}')";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }

    // ----------------- Вивід даних із таблиці -----------------
    static void ShowTableSummary(NpgsqlConnection conn, string tableName, string columns)
    {
        // рахуємо кількість рядків
        string countSql = $"SELECT COUNT(*) FROM {tableName}";
        using var countCmd = new NpgsqlCommand(countSql, conn);
        long count = (long)countCmd.ExecuteScalar();
        Console.WriteLine($"\nТаблиця {tableName}: {count} записів");

        // виводимо вміст таблиці
        string selectSql = $"SELECT {columns} FROM {tableName}";
        using var selectCmd = new NpgsqlCommand(selectSql, conn);
        using var reader = selectCmd.ExecuteReader();

        // шапка таблиці
        for (int i = 0; i < reader.FieldCount; i++)
            Console.Write(reader.GetName(i) + " | ");
        Console.WriteLine("\n" + new string('-', reader.FieldCount * 15));

        // рядки таблиці
        while (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
                Console.Write(reader.GetValue(i) + " | ");
            Console.WriteLine();
        }
        reader.Close();
    }

    // ----------------- Вивід тварин з притулками (JOIN) -----------------
    static void ShowPetsWithShelters(NpgsqlConnection conn)
    {
        Console.WriteLine("\nТаблиця pets (з JOIN shelters):");

        string sql = @"
            SELECT p.pet_id, p.pet_name, p.type, p.age, p.gender,
                   p.description, p.health, p.photourl,
                   s.shelter_name
            FROM pets p
            JOIN shelters s ON p.shelter_id = s.shelter_id
            ORDER BY p.pet_id;
        ";

        using var cmd = new NpgsqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        // шапка
        for (int i = 0; i < reader.FieldCount; i++)
            Console.Write(reader.GetName(i) + " | ");
        Console.WriteLine("\n" + new string('-', reader.FieldCount * 15));

        // рядки
        while (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
                Console.Write(reader.GetValue(i) + " | ");
            Console.WriteLine();
        }
        reader.Close();
    }

    // ----------------- Вивід заявок (JOIN з users і pets) -----------------
    static void ShowAdoptions(NpgsqlConnection conn)
    {
        Console.WriteLine("\nТаблиця adoption_requests (з JOIN):");

        string sql = @"
            SELECT ar.adopt_id, u.user_name || ' ' || u.user_surname AS user_fullname,
                   p.pet_name, ar.date, ar.status
            FROM adoption_requests ar
            JOIN users u ON ar.user_id = u.user_id
            JOIN pets p ON ar.pet_id = p.pet_id
            ORDER BY ar.adopt_id;
        ";

        using var cmd = new NpgsqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        // шапка
        for (int i = 0; i < reader.FieldCount; i++)
            Console.Write(reader.GetName(i) + " | ");
        Console.WriteLine("\n" + new string('-', reader.FieldCount * 15));

        // рядки
        while (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
                Console.Write(reader.GetValue(i) + " | ");
            Console.WriteLine();
        }
        reader.Close();
    }
}
