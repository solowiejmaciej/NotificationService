import http from "k6/http"

export const options = {
    scenarios: {
        issue_perf: {
            executor: "ramping-vus",
            stages: [
                {duration: "60s", target: 100},
                {duration: "35s", target: 0}
            ]
        }
    }
};

export default function () {

    let token = "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImI1YzYwNjFhLWIyNzAtNDZkNi1iZjY5LTEwNmU4MDJlYTE4NSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJtYWNpZWpzb2wxOTI2QGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVzZXIiLCJqdGkiOiI0NjE3MGQ4Ni1lYjE0LTRmNGUtYjI3YS0wMjhjYzI3NDFkYzgiLCJleHAiOjE2OTA0ODAwODAsImlzcyI6Imh0dHA6Ly9ub3RpZmljYXRpb25zZXJ2aWNlLndlc3RldXJvcGUuYXp1cmVjb250YWluZXIuaW8iLCJhdWQiOiJodHRwOi8vbm90aWZpY2F0aW9uc2VydmljZS53ZXN0ZXVyb3BlLmF6dXJlY29udGFpbmVyLmlvIn0.Df7wRvUmrqYCHmVra8BpglE4xBBYswQKBCn03EeEVyrxs3GDmY-zLZV0mH0S9oDP58kxpvtPdPHTjwhE-ILlZw";

    http.get("https://localhost:5000/api/Emails",
        {
            headers: {
                "Authorization": token,
                "X-Api-Key": "7454278F547A491A8A4A061723728F83",
                "Content-Type": "application/json"
            },
        }
    )
}