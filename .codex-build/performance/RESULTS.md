# Import navigation performance validation

Date: 2026-09-15. Read-only comparison against the database configured in
`PlantMobile/appsettings.json`, using the same user and selected date for both queries.
No persistent database tables, indexes, or records were changed.

## Final measured run

| Scenario | Original | Optimized | Result rows |
| --- | ---: | ---: | ---: |
| Request list | 4,309 ms | 2,004 ms | 16 |
| Committees, first request | 15,752 ms | 1,533 ms | 1 |
| Committees, second request | 17,470 ms | 1,599 ms | 4 |
| Missing request | 91 ms | 432 ms | 0 |
| Unassigned user, request list | 53 ms | 226 ms | 0 |

All five comparisons returned identical column values and row multiplicities.
Comparison ignores RowNum for tied committee IDs and normalizes the order of
the two STRING_AGG fields that have no SQL ordering clause. Ordered sample
barcodes are compared exactly. No business data is printed by the harness.

These are individual database load measurements, not browser render timings
or a load test. The original list included 126 ms opening its connection; later
connections used the pool. Empty results incur additional temporary-table setup
cost. The optimized queries use per-connection temporary tables in tempdb;
production concurrency and tempdb capacity were not load-tested.

## Changes

- Filter authorized committees first, using separate request and date paths.
- Resolve the company once per request, after filtering.
- For committee pages, load scoped workflow data into indexed temporary tables
  once instead of repeatedly reading operational tables for each status.
- Preserve the existing request-list projection and all authorization filters.
- Start independent detail reads together after authorization and barcode writes
  complete. Detail-page browser latency was not measured.
- Record slow login and request-list connection/query timings separately.
- Cancel login/list SQL work when the HTTP request is aborted.

## Validation

`dotnet run --project .codex-build/performance/Performance.csproj --no-restore`

The harness reads configuration locally and uses `baseline.sql`, saved from the
working tree before this performance change, to check the old query.

`dotnet build PlantMobile/PlantMobile.csproj --no-restore -o .codex-build/performance/app --verbosity quiet -clp:ErrorsOnly`

Build succeeded with zero errors; existing nullable warnings remain.
The changes have not been deployed or used to restart a running server.
