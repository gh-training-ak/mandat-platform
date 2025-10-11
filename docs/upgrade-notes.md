# Upgrade notes

## 2.0.0

`GET /api/mentors` now returns an envelope:

```diff
- [ { "id": "...", "displayName": "..." } ]
+ {
+   "items": [ { "id": "...", "displayName": "..." } ],
+   "page": 1,
+   "pageSize": 20,
+   "total": 148
+ }
```

Update any client that indexes the response directly. The Angular front end was updated in the
same release.
