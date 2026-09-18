# Tasks: AWS S3 Public Image Storage

- [x] Define the approved scope, public URL model, legacy-URL preservation, authentication requirement, and manual AWS provisioning boundary.
- [x] Add storage options validation and production-safe S3 upload/URL/key behavior.
- [x] Require authentication for image administration endpoints without changing the request/response DTOs.
- [x] Add focused S3 adapter, configuration, and endpoint authorization tests.
- [x] Document IAM, S3 bucket policy, Railway variables, credential rotation, smoke test, and rollback.
- [x] Run focused tests and the Release solution build; record results in `workflow-state.md`.
- [ ] Have an AWS owner apply the IAM and bucket policies, configure Railway secrets, and run the documented production smoke test after deployment.
