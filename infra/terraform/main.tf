terraform {
  required_version = ">= 1.9.0"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.1"
    }
  }

  backend "azurerm" {
    resource_group_name  = "mandat-tfstate"
    storage_account_name = "mandattfstate"
    container_name       = "tfstate"
    key                  = "mandat.tfstate"
  }
}

provider "azurerm" {
  features {}
}

locals {
  name_prefix = "mandat-${var.environment}"

  tags = {
    application = "mandat"
    environment = var.environment
    managed_by  = "terraform"
  }
}

resource "azurerm_resource_group" "this" {
  name     = "${local.name_prefix}-rg"
  location = var.location
  tags     = local.tags
}

module "network" {
  source              = "./modules/network"
  name_prefix         = local.name_prefix
  resource_group_name = azurerm_resource_group.this.name
  location            = azurerm_resource_group.this.location
  address_space       = var.address_space
  tags                = local.tags
}

module "aks" {
  source              = "./modules/aks"
  name_prefix         = local.name_prefix
  resource_group_name = azurerm_resource_group.this.name
  location            = azurerm_resource_group.this.location
  subnet_id           = module.network.aks_subnet_id
  node_count          = var.environment == "prod" ? 5 : 2
  tags                = local.tags
}
