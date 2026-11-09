import unittest
from economy import Attraction, Park

class ParkEconomyTests(unittest.TestCase):
    def test_build_and_operate(self):
        park = Park()
        park.add_attraction(Attraction("Wheel", 10, 20, 30), 200)
        self.assertEqual(park.balance, 800)
        self.assertEqual(park.operate_day({"Wheel": 12}), 170)
        self.assertEqual(park.balance, 970)

    def test_duplicate_is_rejected(self):
        park = Park()
        ride = Attraction("Wheel", 10, 20, 30)
        park.add_attraction(ride, 100)
        with self.assertRaises(ValueError):
            park.add_attraction(ride, 100)

    def test_insufficient_funds(self):
        with self.assertRaises(ValueError):
            Park(balance=10).add_attraction(Attraction("A", 1, 1, 1), 20)

    def test_invalid_visitors_do_not_change_balance(self):
        park = Park()
        with self.assertRaises(ValueError):
            park.operate_day({"missing": 2})
        self.assertEqual(park.balance, 1000)

if __name__ == "__main__":
    unittest.main()
