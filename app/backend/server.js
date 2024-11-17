const express = require('express');
const mongoose = require('mongoose');
const cors = require('cors');
const app = express();
const PORT = 5000;

app.use(cors());
app.use(express.json());

const mongoURI = 'mongodb+srv://asanderson94:6uf8WUl5v6YpanwH@debt-tracker-cluster.iy5d4.mongodb.net/?retryWrites=true&w=majority&appName=debt-tracker-cluster';
mongoose.connect(mongoURI, { useNewUrlParser: true, useUnifiedTopology: true })
  .then(() => console.log('MongoDB Connected'))
  .catch(err => console.log(err));

const debtSchema = new mongoose.Schema({
  creditor: String,
  amount: Number,
  notes: String,
  dateAdded: { type: Date, default: Date.now },
  dateLastEdit: { type: Date, default: Date.now },
  percentageChange: { type: Number, default: null },
});

const Debt = mongoose.model('Debts', debtSchema);

app.get('/api/debts', async (req, res) => {
  const debts = await Debt.find();
  res.json(debts);
});

app.get('/api/debts/:id', async (req, res) => {
  const debt = await Debt.findById(req.params.id);
  res.json(debt);
});

app.put('/api/debts/:id', async (req, res) => {
  try {
    const debtId = req.params.id;
    const updatedDebt = req.body;
    const existingDebt = await Debt.findById(debtId);
    const percentageChange = existingDebt.amount === 0 ? null : ((updatedDebt.amount - existingDebt.amount) / existingDebt.amount) * 100;
    
    updatedDebt.percentageChange = percentageChange;
    updatedDebt.dateLastEdit = new Date();

    const result = await Debt.findByIdAndUpdate(debtId, updatedDebt, { new: true });

    res.json({ message: 'Debt updated successfully', debt: result });
  } catch (error) {
    res.status(500).json({ message: 'Error updating debt', error });
  }
});

app.post('/api/debts', async (req, res) => {
  try {
    const newDebt = new Debt(req.body);
    const savedDebt = await newDebt.save();
    res.status(201).json(savedDebt);
  } catch (error) {
    res.status(500).json({ message: 'Error creating debt', error });
  }
});

app.delete('/api/debts/:id', async (req, res) => {
  try {
    await Debt.findByIdAndDelete(req.params.id);
    res.json({ message: 'Debt deleted successfully' });
  } catch (error) {
    res.status(500).json({ message: 'Error deleting debt', error });
  }
});

app.listen(PORT, () => console.log(`Server running on http://localhost:${PORT}`));
