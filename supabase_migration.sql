-- ============================================================
-- Healthmug Attendance AI - Supabase Migration Script
-- Schema: attendance (public nahi)
-- Naye Supabase project mein SQL Editor mein run karein
-- ============================================================


-- ============================================================
-- STEP 1: attendance schema banao
-- ============================================================
CREATE SCHEMA IF NOT EXISTS attendance;


-- ============================================================
-- TABLE 1: attendance.users
-- Controllers mein use: /rest/v1/users
-- ============================================================
CREATE TABLE IF NOT EXISTS attendance.users (
    id            SERIAL PRIMARY KEY,
    phone_number  VARCHAR(20)    NOT NULL UNIQUE,
    pin           VARCHAR(10)    NOT NULL,
    shift_hours   VARCHAR(10)    NOT NULL DEFAULT '9',
    salary        NUMERIC(10, 2) NOT NULL DEFAULT 0,
    cl_hours      INTEGER        NOT NULL DEFAULT 0,
    el_hours      INTEGER        NOT NULL DEFAULT 0,
    saturday_rule VARCHAR(20)    NOT NULL DEFAULT 'full'
);

-- Index: login ke liye phone_number + pin par fast lookup
CREATE INDEX IF NOT EXISTS idx_users_phone_number
    ON attendance.users (phone_number);

CREATE INDEX IF NOT EXISTS idx_users_phone_pin
    ON attendance.users (phone_number, pin);


-- ============================================================
-- TABLE 2: attendance.attendance
-- Controllers mein use: /rest/v1/attendance
-- ============================================================
CREATE TABLE IF NOT EXISTS attendance.attendance (
    id               SERIAL PRIMARY KEY,
    user_id          INTEGER     NOT NULL REFERENCES attendance.users(id) ON DELETE CASCADE,
    date             VARCHAR(20) NOT NULL,
    in_time          VARCHAR(20) NOT NULL,
    out_time         VARCHAR(20) NOT NULL DEFAULT '',
    duration         INTEGER     NOT NULL DEFAULT 0,
    short_leave_mins INTEGER,
    short_leave_type VARCHAR(20)
);

-- Index: user ke attendance records fast fetch karne ke liye
CREATE INDEX IF NOT EXISTS idx_attendance_user_id
    ON attendance.attendance (user_id);

-- Index: date ke hisaab se filter karne ke liye
CREATE INDEX IF NOT EXISTS idx_attendance_date
    ON attendance.attendance (date);

-- Composite index: user ke specific date ka record dhundhne ke liye
CREATE INDEX IF NOT EXISTS idx_attendance_user_date
    ON attendance.attendance (user_id, date);


-- ============================================================
-- STEP 2: Supabase PostgREST ko attendance schema expose karo
-- (Supabase Dashboard > Settings > API > Extra Search Path mein
--  "attendance" add karo — ya neeche wala command run karo)
-- ============================================================
-- ALTER ROLE authenticator SET search_path TO attendance, public;


-- ============================================================
-- Row Level Security (RLS) - Supabase ke liye zaroori
-- Anon key se read/write allow karne ke liye
-- ============================================================

-- Users table RLS
ALTER TABLE attendance.users ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Allow anon read users"
    ON attendance.users FOR SELECT
    USING (true);

CREATE POLICY "Allow anon insert users"
    ON attendance.users FOR INSERT
    WITH CHECK (true);

CREATE POLICY "Allow anon update users"
    ON attendance.users FOR UPDATE
    USING (true)
    WITH CHECK (true);

CREATE POLICY "Allow anon delete users"
    ON attendance.users FOR DELETE
    USING (true);

-- Attendance table RLS
ALTER TABLE attendance.attendance ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Allow anon read attendance"
    ON attendance.attendance FOR SELECT
    USING (true);

CREATE POLICY "Allow anon insert attendance"
    ON attendance.attendance FOR INSERT
    WITH CHECK (true);

CREATE POLICY "Allow anon update attendance"
    ON attendance.attendance FOR UPDATE
    USING (true)
    WITH CHECK (true);

CREATE POLICY "Allow anon delete attendance"
    ON attendance.attendance FOR DELETE
    USING (true);


-- ============================================================
-- VERIFY: Tables check karne ke liye (optional)
-- ============================================================
-- SELECT table_name, table_schema
-- FROM information_schema.tables
-- WHERE table_schema = 'attendance';
