import React, { useEffect, useState } from 'react';

const DailyCostDisplay = () => {
  const [dailyCost, setDailyCost] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetch('https://localhost:32774/dailycost')
      .then(response => response.json())
      .then(data => {
        setDailyCost(data.dailyCost);
        setLoading(false);
      })
      .catch(error => {
        console.error('Error fetching data:', error);
        setError('Failed to fetch data');
        setLoading(false);
      });
  }, []);

  if (loading) return <div>Loading...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4">Daily Zoo Feeding Costs</h1>
      <h2 className="text-xl font-semibold mb-4">Total Cost: ${dailyCost.totalCost.toFixed(2)}</h2>

      <table className="min-w-full bg-white rounded-lg shadow overflow-hidden">
        <thead className="bg-gray-100">
          <tr>
            <th className="px-4 py-2">Animal</th>
            <th className="px-4 py-2">Species</th>
            <th className="px-4 py-2">Meat (kg)</th>
            <th className="px-4 py-2">Fruit (kg)</th>
            <th className="px-4 py-2">Cost ($)</th>
          </tr>
        </thead>
        <tbody>
          {dailyCost.breakdown.map((item, index) => (
            <tr key={index} className="text-center border-t">
              <td className="px-4 py-2">{item.animal}</td>
              <td className="px-4 py-2">{item.species}</td>
              <td className="px-4 py-2">{item.meatKg.toFixed(2)}</td>
              <td className="px-4 py-2">{item.fruitKg.toFixed(2)}</td>
              <td className="px-4 py-2">{item.cost.toFixed(2)}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default DailyCostDisplay;
