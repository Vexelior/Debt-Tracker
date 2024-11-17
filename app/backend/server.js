const express = require('express');
const mongoose = require('mongoose');
const cors = require('cors');

const app = express();
const PORT = 5000;

// Middleware
app.use(cors());
app.use(express.json());

// MongoDB Connection
const mongoURI = 'mongodb+srv://asanderson94:6uf8WUl5v6YpanwH@debt-tracker-cluster.iy5d4.mongodb.net/?retryWrites=true&w=majority&appName=debt-tracker-cluster';
mongoose.connect(mongoURI, { useNewUrlParser: true, useUnifiedTopology: true })
  .then(() => console.log('MongoDB Connected'))
  .catch(err => console.log(err));

// Schema and Model
const debtSchema = new mongoose.Schema({
  creditor: String,
  amount: Number,
});

const Debt = mongoose.model('Debts', debtSchema);

// Routes
app.get('/api/debts', async (req, res) => {
  const debts = await Debt.find();
  res.json(debts);
});

app.get('/api/debts/:id', async (req, res) => {
  const debt = await Debt.findById(req.params.id);
  res.json(debt);
});

app.get('/api/debts/total', async (req, res) => {
  const debts = await Debt.find();
  const total = debts.reduce((acc, debt) => acc + debt.amount, 0);
  res.json({ total });
});

app.put('/api/debts/:id', async (req, res) => {
  await Debt.findByIdAndUpdate(req.params.id, req.body);
  res.json({ message: 'Debt updated' });
});

app.post('/api/debts', async (req, res) => {
  const debt = new Debt(req.body);
  await debt.save();
  res.json(debt);
});

app.delete('/api/debts/:id', async (req, res) => {
  await Debt.findByIdAndDelete(req.params.id);
  res.json({ message: 'Debt deleted' });
});

app.listen(PORT, () => console.log(`Server running on http://localhost:${PORT}`));
