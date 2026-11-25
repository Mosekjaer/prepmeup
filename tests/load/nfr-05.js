import http from 'k6/http';
import { check } from 'k6';

const baseUrl = __ENV.BASE_URL || 'http://localhost:5001';
const token = __ENV.TOKEN;

export const options = {
  vus: 10,
  duration: '1m',
  thresholds: {
    http_req_duration: ['p(90)<1000'],
    http_req_failed: ['rate==0'],
  },
};

export function setup() {
  if (!token) {
    throw new Error('Set TOKEN to a customer access token before running the test.');
  }
}

export default function () {
  const response = http.get(`${baseUrl}/user`, {
    headers: { Authorization: `Bearer ${token}` },
  });

  check(response, {
    'status is 200': (r) => r.status === 200,
  });
}
