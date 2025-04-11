const User = require('../models/User');

// Get current user (Profile)
exports.getMe = async (req, res) => {
  const user = await User.findById(req.user.id).select('-password');
  res.status(200).json(user);
};

// List all users (admin only - optional)
exports.getAllUsers = async (req, res) => {
  const users = await User.find().select('-password');
  res.json(users);
};
