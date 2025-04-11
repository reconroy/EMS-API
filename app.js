// app.js
const express = require('express');
const dotenv = require('dotenv');
const cors = require('cors');
const connectDB = require('./config/db');

//plugging-in routes here 
const authRoutes = require('./routes/authRoutes');
const userRoutes = require('./routes/userRoutes');

// Load env vars
dotenv.config();

// DB connection
connectDB();

const app = express();

// Middlewares
app.use(cors());
app.use(express.json());

// Routes
app.use('/api/auth', authRoutes);
app.use('/api/user', userRoutes);

// Listen 
const PORT = process.env.PORT || 5000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
