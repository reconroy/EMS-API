const express = require('express');
const router = express.Router();
const { getMe, getAllUsers } = require('../controllers/userController');
const { protect } = require('../middlewares/authMiddleware');

router.get('/me', protect, getMe);
router.get('/all', protect, getAllUsers); // Optional: restrict to admin only

module.exports = router;
