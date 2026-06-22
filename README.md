# Enterprise Maintenance Platform

This project provides a digital, transparent, and efficient system for managing the entire lifecycle of machines and maintenance processes.

## The Problem
Keeping track of industrial machine maintenance on paper or using outdated software is slow, prone to errors, and makes it difficult to know what is actually happening on the shop floor.

## Before vs. After
| Feature | Traditional Approach | Our Solution |
| :--- | :--- | :--- |
| **Asset Lookup** | Searching paper files or spreadsheets | Instant QR code scan |
| **Maintenance Status** | Obscure and delayed | Real-time, transparent dashboard |
| **Audit Trails** | Manual, incomplete logs | Automated, immutable history |
| **Technician UX** | Clunky desktop workflows | Mobile-first, task-oriented app |

## Key Features
- **QR-Scan-First:** Streamlined access to asset details and tasks.
- **Auditable Actions:** Every critical change is logged in an append-only system.
- **Role-Based Access Control (RBAC):** Secure handling of user permissions.
- **Workflow Automation:** Managed state transitions for work orders and checklists.
- **Performance Optimized:** API-first design targeting fast mobile responses (<500ms).

## Architecture
This project follows a modern, scalable architecture:
- **Backend:** ASP.NET Core with Clean Architecture, CQRS-light pattern, and an event-driven design for notifications and workflows.
- **Frontend (Web):** Vue-based interface for management and data overview.
- **Mobile (App):** Flutter-based application for technicians, optimized for field work and scan-first interaction.
