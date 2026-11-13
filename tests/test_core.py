import unittest
from core import Park


class ParkTests(unittest.TestCase):
    def test_budget_and_visitor_capacity(self):
        park = Park(budget=150)
        park.add_path((1, 0))
        park.build((2, 0), cost=100, capacity=5, ticket=10, upkeep=3)
        self.assertEqual(park.balance, 50)
        report = park.run_day(20)
        self.assertEqual(report["served"], 5)
        self.assertEqual(report["revenue"], 50)
        self.assertEqual(report["maintenance"], 3)
        self.assertEqual(report["balance"], 97)

    def test_disconnected_paths_do_not_serve(self):
        park = Park()
        park.add_path((5, 5))
        park.build((5, 6), 20)
        self.assertEqual(park.run_day(20)["served"], 0)

    def test_no_negative_budget_on_build(self):
        park = Park(budget=10)
        with self.assertRaises(ValueError):
            park.build((1, 1), 11)
        self.assertEqual(park.balance, 10)
        self.assertFalse(park.rides)

    def test_reset_and_repeatability(self):
        park = Park()
        park.add_path((1, 0))
        park.build((2, 0), 20)
        first = park.run_day(8)
        park.reset()
        park.add_path((1, 0))
        park.build((2, 0), 20)
        self.assertEqual(park.run_day(8), first)
        with self.assertRaises(ValueError):
            park.run_day(-1)


if __name__ == "__main__":
    unittest.main()
