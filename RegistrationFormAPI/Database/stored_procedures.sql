-- Database creation script for RegistrationFormAPI

-- Create the users table
CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    full_name VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    phone VARCHAR(20) NOT NULL,
    password VARCHAR(64) NOT NULL, -- SHA256 hash is 64 characters
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Function to create a new user
CREATE OR REPLACE FUNCTION create_user(
    p_full_name VARCHAR(100),
    p_email VARCHAR(100),
    p_phone VARCHAR(20),
    p_password VARCHAR(64)
) RETURNS JSON AS $$
DECLARE
    new_id INT;
    result JSON;
BEGIN
    -- Check if email already exists
    IF EXISTS (SELECT 1 FROM users WHERE email = p_email) THEN
        result := json_build_object(
            'success', false,
            'message', 'Email already exists',
            'data', NULL
        );
        RETURN result;
    END IF;

    -- Insert the new user
    INSERT INTO users (full_name, email, phone, password)
    VALUES (p_full_name, p_email, p_phone, p_password)
    RETURNING id INTO new_id;

    -- Return success response
    result := json_build_object(
        'success', true,
        'message', 'User created successfully',
        'data', json_build_object(
            'id', new_id
        )
    );

    RETURN result;
EXCEPTION WHEN OTHERS THEN
    result := json_build_object(
        'success', false,
        'message', 'Error creating user: ' || SQLERRM,
        'data', NULL
    );
    RETURN result;
END;
$$ LANGUAGE plpgsql;

-- Function to get all users (without returning passwords)
CREATE OR REPLACE FUNCTION get_all_users() RETURNS JSON AS $$
DECLARE
    user_list JSON;
    result JSON;
BEGIN
    SELECT json_agg(
        json_build_object(
            'id', u.id,
            'fullName', u.full_name,
            'email', u.email,
            'phone', u.phone,
            'createdAt', u.created_at
        )
    ) INTO user_list
    FROM users u;

    -- Handle empty result
    IF user_list IS NULL THEN
        user_list := '[]'::JSON;
    END IF;

    result := json_build_object(
        'success', true,
        'message', 'Users retrieved successfully',
        'data', user_list
    );

    RETURN result;
EXCEPTION WHEN OTHERS THEN
    result := json_build_object(
        'success', false,
        'message', 'Error retrieving users: ' || SQLERRM,
        'data', NULL
    );
    RETURN result;
END;
$$ LANGUAGE plpgsql;

-- Function to get user by ID (without returning password)
CREATE OR REPLACE FUNCTION get_user_by_id(p_id INT) RETURNS JSON AS $$
DECLARE
    user_data JSON;
    result JSON;
BEGIN
    SELECT json_build_object(
        'id', u.id,
        'fullName', u.full_name,
        'email', u.email,
        'phone', u.phone,
        'createdAt', u.created_at
    ) INTO user_data
    FROM users u
    WHERE u.id = p_id;

    -- Check if user exists
    IF user_data IS NULL THEN
        result := json_build_object(
            'success', false,
            'message', 'User not found',
            'data', NULL
        );
    ELSE
        result := json_build_object(
            'success', true,
            'message', 'User retrieved successfully',
            'data', user_data
        );
    END IF;

    RETURN result;
EXCEPTION WHEN OTHERS THEN
    result := json_build_object(
        'success', false,
        'message', 'Error retrieving user: ' || SQLERRM,
        'data', NULL
    );
    RETURN result;
END;
$$ LANGUAGE plpgsql;

-- Function to update user
CREATE OR REPLACE FUNCTION update_user(
    p_id INT,
    p_full_name VARCHAR(100),
    p_email VARCHAR(100),
    p_phone VARCHAR(20),
    p_password VARCHAR(64) DEFAULT NULL
) RETURNS JSON AS $$
DECLARE
    result JSON;
    user_exists BOOLEAN;
    email_exists BOOLEAN;
BEGIN
    -- Check if user exists
    SELECT EXISTS(SELECT 1 FROM users WHERE id = p_id) INTO user_exists;
    
    IF NOT user_exists THEN
        result := json_build_object(
            'success', false,
            'message', 'User not found',
            'data', NULL
        );
        RETURN result;
    END IF;

    -- Check if email already exists for another user
    SELECT EXISTS(SELECT 1 FROM users WHERE email = p_email AND id != p_id) INTO email_exists;
    
    IF email_exists THEN
        result := json_build_object(
            'success', false,
            'message', 'Email already exists for another user',
            'data', NULL
        );
        RETURN result;
    END IF;

    -- Update based on whether password is provided
    IF p_password IS NULL THEN
        UPDATE users 
        SET 
            full_name = p_full_name,
            email = p_email,
            phone = p_phone
        WHERE id = p_id;
    ELSE
        UPDATE users 
        SET 
            full_name = p_full_name,
            email = p_email,
            phone = p_phone,
            password = p_password
        WHERE id = p_id;
    END IF;

    result := json_build_object(
        'success', true,
        'message', 'User updated successfully',
        'data', NULL
    );
    
    RETURN result;
EXCEPTION WHEN OTHERS THEN
    result := json_build_object(
        'success', false,
        'message', 'Error updating user: ' || SQLERRM,
        'data', NULL
    );
    RETURN result;
END;
$$ LANGUAGE plpgsql;

-- Function to delete user
CREATE OR REPLACE FUNCTION delete_user(p_id INT) RETURNS JSON AS $$
DECLARE
    result JSON;
    user_exists BOOLEAN;
BEGIN
    -- Check if user exists
    SELECT EXISTS(SELECT 1 FROM users WHERE id = p_id) INTO user_exists;
    
    IF NOT user_exists THEN
        result := json_build_object(
            'success', false,
            'message', 'User not found',
            'data', NULL
        );
        RETURN result;
    END IF;

    -- Delete the user
    DELETE FROM users WHERE id = p_id;

    result := json_build_object(
        'success', true,
        'message', 'User deleted successfully',
        'data', NULL
    );
    
    RETURN result;
EXCEPTION WHEN OTHERS THEN
    result := json_build_object(
        'success', false,
        'message', 'Error deleting user: ' || SQLERRM,
        'data', NULL
    );
    RETURN result;
END;
$$ LANGUAGE plpgsql;