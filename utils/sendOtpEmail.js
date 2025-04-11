const nodemailer = require('nodemailer');

const sendOtpEmail = async (email, otp) => {
  const transporter = nodemailer.createTransport({
    service: 'Gmail', // or any SMTP service like Mailtrap, SendGrid, etc.
    auth: {
      user: process.env.EMAIL_USER,
      pass: process.env.EMAIL_PASS,
    },
  });

  const message = {
    from: '"EMS System" <no-reply@ems.com>',
    to: email,
    subject: 'Your OTP Code',
    html: `<h3>Your OTP is: <strong>${otp}</strong></h3><p>This code will expire in 5 minutes.</p>`,
  };

  await transporter.sendMail(message);
};

module.exports = sendOtpEmail;
