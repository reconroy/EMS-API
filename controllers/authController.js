const User = require('../models/User');
const jwt = require('jsonwebtoken');
const Otp = require('../models/Otp');
const sendOtpEmail = require('../utils/sendOtpEmail');

// Register
exports.registerUser = async (req, res) => {
    const { name, email, password } = req.body;

    try {
        const userExists = await User.findOne({ email });
        if (userExists) return res.status(400).json({ message: 'User already exists' });

        const user = await User.create({ name, email, password });

        res.status(201).json({
            _id: user._id,
            name: user.name,
            email: user.email,
            token: generateToken(user._id),
        });
    } catch (err) {
        res.status(500).json({ message: 'Something went wrong', error: err.message });
    }
};

// Login
exports.loginUser = async (req, res) => {
    const { email, password } = req.body;

    try {
        const user = await User.findOne({ email });
        if (user && (await user.matchPassword(password))) {
            res.json({
                _id: user._id,
                name: user.name,
                email: user.email,
                token: generateToken(user._id),
            });
        } else {
            res.status(401).json({ message: 'Invalid credentials' });
        }
    } catch (err) {
        res.status(500).json({ message: 'Something went wrong' });
    }
};

// Generate JWT
const generateToken = (id) => {
    return jwt.sign({ id }, process.env.JWT_SECRET, { expiresIn: '30d' });
};

exports.sendOtp = async (req, res) => {
    const { email } = req.body;

    try {
        const userExists = await User.findOne({ email });
        if (userExists) return res.status(400).json({ message: 'Email already registered' });

        const otpCode = Math.floor(100000 + Math.random() * 900000).toString();
        const expiryMinutes = parseInt(process.env.OTP_EXPIRY_MINUTES || '5');
        const expiresAt = new Date(Date.now() + expiryMinutes * 60000); // from env

        await Otp.findOneAndUpdate(
            { email },
            { otp: otpCode, expiresAt },
            { upsert: true, new: true }
        );

        await sendOtpEmail(email, otpCode);

        res.status(200).json({ message: 'OTP sent successfully' });
    } catch (err) {
        res.status(500).json({ message: 'Failed to send OTP', error: err.message });
    }
};

exports.verifyOtpOnly = async (req, res) => {
    const { email, otp } = req.body;

    try {
        const otpRecord = await Otp.findOne({ email });
        if (!otpRecord) return res.status(400).json({ message: 'OTP not found' });

        if (otpRecord.expiresAt < Date.now())
            return res.status(400).json({ message: 'OTP expired' });

        if (otpRecord.otp !== otp)
            return res.status(400).json({ message: 'Invalid OTP' });

        res.status(200).json({ message: 'OTP verified successfully' });
    } catch (err) {
        res.status(500).json({ message: 'OTP verification failed', error: err.message });
    }
};

exports.verifyOtpAndRegister = async (req, res) => {
    const { name, email, password } = req.body;
  
    try {
      const otpRecord = await Otp.findOne({ email });
      if (!otpRecord)
        return res.status(400).json({ message: 'OTP not verified or expired' });
  
      const userExists = await User.findOne({ email });
      if (userExists)
        return res.status(400).json({ message: 'Email already registered' });
  
      const newUser = await User.create({ name, email, password });
  
      await Otp.deleteOne({ email });
  
      res.status(201).json({
        _id: newUser._id,
        name: newUser.name,
        email: newUser.email,
        token: generateToken(newUser._id),
      });
    } catch (err) {
      res.status(500).json({ message: 'Registration failed', error: err.message });
    }
  };

