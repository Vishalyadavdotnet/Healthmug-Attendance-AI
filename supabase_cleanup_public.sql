-- ============================================================
-- PUBLIC SCHEMA CLEANUP SCRIPT
-- Galti se public schema mein bani tables ko delete karne ke liye
-- Supabase SQL Editor mein run karein
-- ============================================================


-- STEP 1: public.attendance ki saari policies drop karo
DROP POLICY IF EXISTS "Allow anon read attendance"   ON public.attendance;
DROP POLICY IF EXISTS "Allow anon insert attendance" ON public.attendance;
DROP POLICY IF EXISTS "Allow anon update attendance" ON public.attendance;
DROP POLICY IF EXISTS "Allow anon delete attendance" ON public.attendance;

-- STEP 2: public.users ki saari policies drop karo
DROP POLICY IF EXISTS "Allow anon read users"   ON public.users;
DROP POLICY IF EXISTS "Allow anon insert users" ON public.users;
DROP POLICY IF EXISTS "Allow anon update users" ON public.users;
DROP POLICY IF EXISTS "Allow anon delete users" ON public.users;

-- STEP 3: Tables drop karo (attendance pehle — foreign key hai)
DROP TABLE IF EXISTS public.attendance;
DROP TABLE IF EXISTS public.users;


-- ============================================================
-- VERIFY: Confirm karo ki tables delete ho gayi
-- ============================================================
-- SELECT table_name, table_schema
-- FROM information_schema.tables
-- WHERE table_schema = 'public'
--   AND table_name IN ('users', 'attendance');
-- (Koi row nahi aani chahiye)
