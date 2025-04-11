const express = require('express');
const router = express.Router();
const { registerUser, loginUser, sendOtp, verifyOtpOnly, verifyOtpAndRegister} = require('../controllers/authController');

// Authentication Routes
router.post('/register', registerUser);
router.post('/login', loginUser);
router.post('/send-otp', sendOtp); // Step 1
router.post('/verify-otp', verifyOtpOnly); // Step 2
router.post('/register-after-otp', verifyOtpAndRegister); // Step 3

module.exports = router;
