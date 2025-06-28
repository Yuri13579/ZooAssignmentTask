import React, { useEffect, useState } from 'react';
import { Container, Typography, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, CircularProgress, Alert } from '@mui/material';

const DailyCostDisplay = () => {
  const [dailyCost, setDailyCost] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

useEffect(() => {
  const fetchData = (retryCount = 0) => {
    fetch('https://localhost:32774/dailycost')
      .then(response => response.json())
      .then(data => {
        setDailyCost(data.dailyCost);
        setLoading(false);
      })
      .catch(error => {
        console.error('Error fetching data:', error);
        if (retryCount < 3) {  // retries up to 3 times
          setTimeout(() => fetchData(retryCount + 1), 3000); // retry after 3 seconds
        } else {
          setError('Failed to fetch data');
          setLoading(false);
        }
      });
  };

  fetchData();
}, []);

  if (loading) return <CircularProgress />;
  if (error) return <Alert severity="error">{error}</Alert>;

  return (
    <Container>
      <Typography variant="h4" gutterBottom>Daily Zoo Feeding Costs</Typography>
      <Typography variant="h6" gutterBottom>Total Cost: ${dailyCost.totalCost.toFixed(2)}</Typography>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Animal</TableCell>
              <TableCell>Species</TableCell>
              <TableCell align="right">Meat (kg)</TableCell>
              <TableCell align="right">Fruit (kg)</TableCell>
              <TableCell align="right">Cost ($)</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {dailyCost.breakdown.map((item, index) => (
              <TableRow key={index}>
                <TableCell>{item.animal}</TableCell>
                <TableCell>{item.species}</TableCell>
                <TableCell align="right">{item.meatKg.toFixed(2)}</TableCell>
                <TableCell align="right">{item.fruitKg.toFixed(2)}</TableCell>
                <TableCell align="right">{item.cost.toFixed(2)}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Container>
  );
};

export default DailyCostDisplay;
